
using System;

public class HttpException : Exception
{
    
    public string ErrCode;
    
    public string ErrMessage;
    
    public int Status;

    public HttpException(string errCode,string errMessage,int status) : base()
    {
        this.ErrCode = errCode;
        this.ErrMessage = errMessage;
        this.Status = status;
    }
}
 