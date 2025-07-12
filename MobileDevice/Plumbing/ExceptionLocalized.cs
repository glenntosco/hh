using System;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public class ExceptionLocalized:Exception
    {
        public ExceptionLocalized(string message) : base(Lang.Translate(message))
        {

        }

        public ExceptionLocalized(string message, Exception innerException) : base(Lang.Translate(message), innerException)
        {
        }
    }
}
