using System.Text.Json;
using WebAPI.Data.Models;

namespace WebAPI.Exceptions
{
    public class ModelStateInvalidException<T> : DataException
        where T : Dto
    {
        public new readonly string ERROR_TITLE = "ModelState is invalid";

        private T? _data = null;

        public ModelStateInvalidException() { }
        public ModelStateInvalidException(string message) : base(message) { }
        public ModelStateInvalidException(string message, Exception innerException) : base(message, innerException) { }
        
        public ModelStateInvalidException(string message, T data) : base(message)
        {
            _data = data;
        }
        
        public new ErrorObject GetErrorObject()
        {
            return new ErrorObject(ERROR_TITLE, Message, new { ModelData = _data });
        }

    }
}
