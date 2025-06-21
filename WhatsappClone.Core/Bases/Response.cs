using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Bases;

public class Response<T>
{
    public Response()
    {

    }
    public Response(T data, string message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }
    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }
    public Response(string message, bool succeeded)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public HttpStatusCode StatusCode { get; set; }
    public object Meta { get; set; }

    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    //public Dictionary<string, List<string>> ErrorsBag { get; set; }
    public T Data { get; set; }
}




public class ResponseHandler
{

    public ResponseHandler()
    {

    }
    public Response<T> Deleted<T>(string msg = "Deleted Successfully")
    {
        return new Response<T>()
        {
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = msg
        };
    }
    public Response<T> Success<T>(T entity, string msg = "Retrieved Successfully", object Meta = null)
    {
        return new Response<T>()
        {
            Data = entity,
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = msg,
            Meta = Meta
        };
    }
    public Response<T> Unauthorized<T>(string msg = "UnAuthorized")
    {
        return new Response<T>()
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = true,
            Message = msg
        };
    }
    public Response<T> BadRequest<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = Message == null ? "Bad Request" : Message
        };
    }


    public Response<T> Unproccess<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Succeeded = false,
            Message = Message == null ? "Unprocessable Entity" : Message
        };
    }

    public Response<T> NotFound<T>(string message = null)
    {
        return new Response<T>()
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = message == null ? "Not Found" : message
        };
    }

    public Response<T> Created<T>(T entity, object Meta = null)
    {
        return new Response<T>()
        {
            Data = entity,
            StatusCode = HttpStatusCode.Created,
            Succeeded = true,
            Message = "Created",
            Meta = Meta
        };
    }
}
