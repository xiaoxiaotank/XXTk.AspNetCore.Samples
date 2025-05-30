namespace System;
public class BusinessException : Exception, IBusinessException
{
    public BusinessException()
    {
    }
    public BusinessException(string message, int code = 0) : base(message)
    {
        Code = code;
    }
    public int Code { get; }
}