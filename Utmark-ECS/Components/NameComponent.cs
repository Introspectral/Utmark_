using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components
{
    public class NameComponent : IComponent
    {
        public string Name { get; set; }
        public NameComponent(string name)
        {
            Name = name;
        }

    }
}
