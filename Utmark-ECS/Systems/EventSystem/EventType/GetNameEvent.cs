using Utmark_ECS.Components;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class GetNameEvent
    {
        public NameComponent Name { get; }

        public GetNameEvent(NameComponent name)
        {
            Name = name;
        }
    }
}
