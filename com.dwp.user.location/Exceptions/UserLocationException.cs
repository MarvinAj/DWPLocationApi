
namespace com.dwp.user.location.Exceptions
{
    public class UserLocationException : Exception
    {
        public UserLocationException(string message)
            : base(message)
        { }

        public UserLocationException(string message, HttpResponseMessage httpResponseMessage)
            : base(
                  message,
                  new Exception($"{httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}"))
        {
        }
    }
}
