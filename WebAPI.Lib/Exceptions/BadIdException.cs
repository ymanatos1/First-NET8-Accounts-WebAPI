using System.Text.Json;
using WebAPI.Data.Models;

namespace WebAPI.Exceptions
{
    public class BadIdException : DataException
    {
        public new readonly string ERROR_TITLE = "Bad Id Exception";

        //private Account? _data = null;
        private int? _id = null;

        public BadIdException() { }
        public BadIdException(string message) : base(message) { }
        public BadIdException(string message, Exception innerException) : base(message, innerException) { }
        
        public BadIdException(string message, int id) : base(message)
        {
            _id = id;
        }
        
        public new ErrorObject GetErrorObject()
        {
            return new ErrorObject(ERROR_TITLE, Message, new { Id = _id });
        }

    }
}
