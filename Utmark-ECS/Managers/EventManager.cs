namespace Utmark_ECS.Managers
{
    public class EventManager
    {
        private readonly Dictionary<Type, Delegate> _eventHandlers = new();

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class
        {
            var eventType = typeof(TEvent);
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = handler;
            }
            else
            {
                _eventHandlers[eventType] = Delegate.Combine(_eventHandlers[eventType], handler);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : class
        {
            var eventType = typeof(TEvent);
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);
                if (_eventHandlers[eventType] is null)
                {
                    _eventHandlers.Remove(eventType);
                }
            }
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : class
        {
            var eventType = typeof(TEvent);
            if (_eventHandlers.TryGetValue(eventType, out var delegateInstance))
            {
                var handler = delegateInstance as Action<TEvent>;
                handler?.Invoke(eventData);
            }
        }
    }

}
