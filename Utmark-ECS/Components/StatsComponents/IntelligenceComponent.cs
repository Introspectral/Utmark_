using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
