using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utmark.Mediator
{
    // Example Request Message
    public class ExampleRequest : IRequest<string> // string is the response type
    {
        public string Data { get; }

        public ExampleRequest(string data)
        {
            Data = data;
        }
    }

    // Example Notification Message
    public class ExampleNotification : INotification
    {
        public string Data { get; }

        public ExampleNotification(string data)
        {
            Data = data;
        }
    }
}
