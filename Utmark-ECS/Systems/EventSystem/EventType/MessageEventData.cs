namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MessageEventData
    {
        public class MessagesEventData
        {
            public string Message { get; }


            public MessagesEventData(string message)
            {
                Message = message.ToString();

            }
        }
    }
}
