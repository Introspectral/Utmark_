namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class MouseScrollEventData
    {
        public int Delta { get; }

        public MouseScrollEventData(int delta)
        {
            Delta = delta;
        }
    }
}
