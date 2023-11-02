using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
