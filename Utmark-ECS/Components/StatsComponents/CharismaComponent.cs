using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class CharismaComponent : IComponent
    {
        public int Charisma { get; set; }
        public CharismaComponent(int charisma)
        {
            Charisma = charisma;
        }
    }
}
