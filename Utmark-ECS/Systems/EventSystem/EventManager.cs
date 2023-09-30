using Utmark_ECS.Systems.EventSystem.EventType;
using static Utmark_ECS.Enums.EventTypeEnum;

namespace Utmark_ECS.Systems.EventSystem
{
    public class EventManager
    {
        // Dictionary to hold handlers for each event type
        private readonly Dictionary<EventTypes, List<Action<EventData>>> _eventHandlers = new();

        // Method to subscribe to an event
        public void Subscribe(EventTypes eventType, Action<EventData> handler)
        {
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = new List<Action<EventData>>();
            }
            _eventHandlers[eventType].Add(handler);
        }

        // Method to unsubscribe from an event
        public void Unsubscribe(EventTypes eventType, Action<EventData> handler)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType].Remove(handler);
                if (_eventHandlers[eventType].Count == 0)
                {
                    _eventHandlers.Remove(eventType);
                }
            }
        }

        // Method to publish an event
        public void Publish(EventTypes eventType, object sender, object data)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                var eventData = new EventData(eventType, sender, data);
                foreach (var handler in _eventHandlers[eventType])
                {
                    handler.Invoke(eventData);
                }
            }
        }
    }

}
