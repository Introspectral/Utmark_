using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class WillpowerComponent : IComponent
    {
        public int Willpower { get; set; }
        public WillpowerComponent(int willpower)
        {
            Willpower = willpower;
        }
    }
}
