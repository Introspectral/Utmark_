using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Intefaces;

namespace Utmark_ECS.Components.StatsComponents
{
    public class ConstitutionComponent : IComponent
    {
        public int Constitution { get; set; }
        public ConstitutionComponent(int constitution) 
        { 
            Constitution = constitution;
        }
    }
}
