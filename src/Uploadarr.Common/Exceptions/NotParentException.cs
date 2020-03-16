
using System;

namespace Uploadarr.Common
{
    public class NotParentException : ApplicationException
    {
        public NotParentException(string message, params object[] args)
            : base(string.Format(message, args))
        {

        }

        public NotParentException(string message)
            : base(message)
        {

        }

        public NotParentException(string message, Exception innerException, params object[] args)
            : base(string.Format(message, args), innerException)
        {

        }

        public NotParentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
