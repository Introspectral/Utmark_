using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class IntelligenceComponent : IComponent
    {
        public int Intelligence { get; set; }
        public IntelligenceComponent(int intelligence)
        {
            Intelligence = intelligence;
        }
    }
}
