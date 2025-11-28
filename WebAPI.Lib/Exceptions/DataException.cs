using WebAPI.Data.Models;

namespace WebAPI.Exceptions
{
    public class DataException : Exception
    {
        public readonly string ERROR_TITLE = "Data Exception";

        public DataException() { }
        public DataException(string message) : base(message) { }
        public DataException(string message, Exception innerException) : base(message, innerException) { }

        public ErrorObject GetErrorObject()
        {
            return new ErrorObject(ERROR_TITLE, Message);
        }

    }

}
