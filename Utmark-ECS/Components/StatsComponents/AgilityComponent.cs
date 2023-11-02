using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class AgilityComponent : IComponent
    {
        public int Agiility { get; set; }
        public AgilityComponent(int agility) 
        { 
            Agiility = agility;
        }
    }
}

