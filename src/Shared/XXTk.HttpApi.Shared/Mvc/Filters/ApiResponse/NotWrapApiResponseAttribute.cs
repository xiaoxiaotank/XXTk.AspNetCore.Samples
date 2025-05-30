namespace XXTk.HttpApi.Shared.Mvc.Filters.ApiResponse;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class NotWrapApiResponseAttribute : Attribute
{
}
