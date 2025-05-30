namespace System;
public interface IBusinessException
{
    string Message { get; }
    int Code { get; }
}