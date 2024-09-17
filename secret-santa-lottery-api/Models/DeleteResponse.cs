using System.Net;

namespace secret_santa_lottery_api.Models;

public class DeleteResponse
{
    public HttpStatusCode StatusCode { get; }

    public DeleteResponse(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
}
