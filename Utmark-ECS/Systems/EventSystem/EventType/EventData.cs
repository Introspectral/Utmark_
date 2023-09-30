using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class EventData
    {
        public EventTypes Type { get; set; }
        public object Sender { get; }
        public object Data { get; }

        public EventData(EventTypes eventType, object sender, object data)
        {
            Type = eventType;
            Sender = sender;
            Data = data;
        }
    }
}
