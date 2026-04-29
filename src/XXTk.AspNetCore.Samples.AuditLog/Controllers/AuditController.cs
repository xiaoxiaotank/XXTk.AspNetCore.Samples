using Audit.Core;
using Microsoft.AspNetCore.Mvc;

namespace XXTk.AspNetCore.Samples.AuditLog.Controllers;

[ApiController]
[Route("[controller]")]
public class AuditController : ControllerBase
{
    private readonly IAuditScopeFactory _auditScopeFactory;

    public AuditController(IAuditScopeFactory auditScopeFactory)
    {
        _auditScopeFactory = auditScopeFactory;
    }

    /// <summary>
    /// 方式一：使用 AuditScopeFactory 创建审计范围（推荐）
    /// 通过依赖注入 IAuditScopeFactory，可以更方便地进行测试和替换实现
    /// </summary>
    [HttpGet("audit-by-factory")]
    public async Task<string> AuditByFactoryAsync()
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 30
        };

        // 创建一个审计范围，使用工厂方法，并传入一些选项
        using (var auditScope = await _auditScopeFactory.CreateAsync(new AuditScopeOptions()
        {
            // 事件类型/名称，可以根据实际业务需求进行设置
            EventType = "TestEvent",
            // 获取目标对象，这个对象会被序列化并记录在审计日志中
            TargetGetter = () => person,
            // 需要记录的额外字段信息，是匿名对象，这些字段会被合并到审计事件中
            ExtraFields = new
            {
                CustomField1 = "Value1",
                CustomField2 = 123,
                // 也可以嵌套对象
                Metadata = new
                {
                    Source = "WebAPI",
                    Version = "1.0"
                }
            }
        }))
        {
            // 在审计过程中，根据业务逻辑动态添加自定义字段
            auditScope.SetCustomField("DynamicField1", "Value1");
            auditScope.SetCustomField("DynamicField2", 100);
            auditScope.SetCustomField("ConditionalField", DateTime.Now);

            // 在父范围中获取 Items（通过 scope.Event.Items 或 scope.GetItems()）
            var parentTraceId = auditScope.Event.CustomFields["CustomField1"];

            // 修改 Items，子范围会看到修改后的值
            auditScope.Event.CustomFields["CustomField2"] = "12334";

            // 在这个范围内执行一些操作，这些操作会被审计记录下来
            // 例如，可以修改 person 对象的属性，这些修改也会被记录
            person.Age = 31;
        }

        return "done";
    }

    /// <summary>
    /// 方式二：使用静态 AuditScope.Create 创建审计范围
    /// 直接调用 AuditScope.Create 静态方法创建审计范围
    /// </summary>
    [HttpGet("audit-by-static")]
    public async Task<string> AuditByStaticAsync()
    {
        var person = new Person
        {
            Name = "John Doe",
            Age = 30
        };

        // 使用 Fluent API 的方式创建审计范围
        // 第一个参数：事件类型
        // 第二个参数：目标对象Getter
        // 第三个参数：额外字段
        using (var scope = await AuditScope.CreateAsync("TestEvent", () => person, new { CreateTime = DateTime.Now }))
        {
            // 在这个范围内执行一些操作，这些操作会被审计记录下来
            person.Age = 31;

            // 通过 Event.CustomFields 动态添加自定义字段
            scope.Event.CustomFields["CustomFields1"] = "1";
        }

        return "done";
    }

    /// <summary>
    /// 方式三：使用 AuditScope.Log 快速记录一次性事件
    /// 当你只需要记录一个简单的事件，不需要追踪对象变更时使用
    /// </summary>
    [HttpGet("audit-by-static-log")]
    public string AuditByStaticLog()
    {
        // AuditScope.Log 是最简单的方式，直接记录一个事件
        // 第一个参数：事件类型
        // 第二个参数：额外数据（可选）
        AuditScope.Log("QuickEvent", new { Operation = "QuickLog", Result = "Success" });

        return "done";
    }

    /// <summary>
    /// 方式四：使用 EventCreationPolicy.Manual 手动控制审计时机
    /// 默认情况下，审计事件会在 using 块结束时自动保存
    /// 设置为 Manual 后，需要手动调用 Save() 方法保存审计事件
    /// </summary>
    [HttpGet("audit-manual-policy")]
    public async Task<string> AuditManualPolicyAsync()
    {
        var person = new Person { Name = "Manual", Age = 25 };

        // CreationPolicy.Manual：需要手动调用 Save() 才能保存审计事件
        using (var scope = await _auditScopeFactory.CreateAsync(new AuditScopeOptions()
        {
            EventType = "ManualPolicyEvent",
            TargetGetter = () => person,
            CreationPolicy = EventCreationPolicy.Manual
        }))
        {
            person.Age = 26;

            // 使用 scope.Save() 手动保存审计事件
            // 这样可以在满足特定条件时才保存，例如业务操作成功后才记录
            scope.Save();

            // 如果不想保存，可以调用 scope.Discard() 丢弃审计事件
            // scope.Discard();
        }

        return "done";
    }

    /// <summary>
    /// 方式五：在 using 块内抛异常时的审计行为
    /// 默认策略（InsertOnEnd）下，即使抛异常，Dispose() 仍会调用 Save()
    /// Target.New 会记录异常抛出时刻的目标对象状态
    /// </summary>
    [HttpGet("audit-with-exception")]
    public async Task<string> AuditWithExceptionAsync()
    {
        var person = new Person { Name = "Exception", Age = 30 };

        using (var scope = await _auditScopeFactory.CreateAsync(new AuditScopeOptions()
        {
            EventType = "ExceptionEvent",
            TargetGetter = () => person
        }))
        {
            person.Age = 31;

            // 在 using 块内抛异常
            // C# 的 using 会转换为 try-finally，Dispose() 一定会被调用
            // 默认策略 InsertOnEnd 会在 Dispose() 时调用 Save()
            // 所以即使抛异常，审计事件依然会被保存
            throw new InvalidOperationException("模拟业务异常");
        }
    }

    /// <summary>
    /// 方式六：捕获异常后手动调用 Discard() 不记录审计
    /// 使用 Manual 策略 + try-catch + Discard()，实现异常时不记录审计
    /// </summary>
    [HttpGet("audit-catch-and-discard")]
    public async Task<string> AuditCatchAndDiscardAsync()
    {
        var person = new Person { Name = "CatchDiscard", Age = 30 };

        using (var scope = await _auditScopeFactory.CreateAsync(new AuditScopeOptions()
        {
            EventType = "CatchDiscardEvent",
            TargetGetter = () => person,
            CreationPolicy = EventCreationPolicy.Manual
        }))
        {
            try
            {
                person.Age = 31;
                throw new InvalidOperationException("模拟业务异常");
            }
            catch (Exception)
            {
                // 捕获异常后调用 Discard() 丢弃审计事件
                // 这样异常不会被记录到审计日志中
                scope.Discard();
                throw;
            }

            // 由于上面会抛出异常，这行代码不会执行
            scope.Save();
        }
    }
}
