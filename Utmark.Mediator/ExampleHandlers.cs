using MediatR;

namespace Utmark.Mediator
{
    public class ExampleRequestHandler : IRequestHandler<ExampleRequest, string>
    {
        public Task<string> Handle(ExampleRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Received: {request.Data}");
        }
    }

    public class ExampleNotificationHandler : INotificationHandler<ExampleNotification>
    {
        public Task Handle(ExampleNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Notification Received: {notification.Data}");
            return Task.CompletedTask;
        }
    }
}