using System.Collections.ObjectModel;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using WebAPI.Data.Models;

namespace WebAPI.Exceptions
{
    public class DataNotFoundException : DataException
    {
        public new readonly string ERROR_TITLE = "Data Not Found";

        //private Account? _data = null;
        private int? _id = null;
        private object? _data = null;

        public DataNotFoundException() { }
        public DataNotFoundException(string message) : base(message) { }
        public DataNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        
        public DataNotFoundException(string message, int id) : base(message)
        {
            _id = id;
        }
        public DataNotFoundException(string message, object data) : base(message)
        {
            _data = data;
        }

        public new ErrorObject GetErrorObject()
        {
            IList<object> objects = new List<object>();

            if (_id != null) objects.Add(new { Id = _id });
            if (_data != null) objects.Add(new { Data = _data });

            switch (objects.Count)
            {
                case 1:
                    return new ErrorObject(ERROR_TITLE, Message, objects.ElementAt(0));
                default:
                    return new ErrorObject(ERROR_TITLE, Message, objects);
            }
        }

    }
}
