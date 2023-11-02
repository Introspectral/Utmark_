namespace Utmark_ECS.Systems.EventSystem.EventType
{

    public class MessageEventData
    {
        public object Sender { get; }
        public string Message { get; }

        public MessageEventData(object sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
