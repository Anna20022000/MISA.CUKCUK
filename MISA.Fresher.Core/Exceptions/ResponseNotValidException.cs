using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.Exceptions
{
    public class ResponseNotValidException : Exception
    {
        public ResponseNotValidException(object? value = null) =>
        (Value) = (value);

        public object? Value { get; }
    }
}
