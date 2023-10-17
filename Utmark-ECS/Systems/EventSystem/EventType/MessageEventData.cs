namespace Utmark_ECS.Systems.EventSystem.EventType
{

    public class MessagesEvent
    {
        public string Message { get; }

        public MessagesEvent(string message)
        {
            Message = message.ToString();

        }
    }

}
