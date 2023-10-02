namespace Utmark_ECS.Systems.EventHandlers
{

    public class MessageEvent
    {
        public object Sender { get; }
        public string Message { get; }

        public MessageEvent(object sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
