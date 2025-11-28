using System.Text.Json;

namespace WebAPI.Data.Models
{
    public class ErrorObject
    {
        public string Error { get; }
        public string Message { get; }
        public object? Data { get; }

        public ErrorObject(string error, string message, object? data = null)
        { 
            Error = error;
            Message = message;
            Data = data;
        }

        public string AsString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
