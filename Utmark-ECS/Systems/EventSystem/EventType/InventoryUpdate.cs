using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utmark_ECS.Components;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType
{
    public class InventoryUpdate
    {
        public ItemComponent Item {get;set;} 

        public InventoryUpdate(ItemComponent Items) 
        { 
            Item = Items;
        }

    }
}
