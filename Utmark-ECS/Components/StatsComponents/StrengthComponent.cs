using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class StrengthComponent : IComponent
    {
        public int Strength { get; set; }

        public StrengthComponent(int strength)
        {
            Strength = strength;
        }
    }
}
