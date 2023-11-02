using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
