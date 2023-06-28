using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Application.Common.Exceptions;
public class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
        : base()
    {
    }

    public AlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public AlreadyExistsException(string tagName)
        : base($"Tag \"{tagName}\" already exists on the item.")
    {
    }
}
