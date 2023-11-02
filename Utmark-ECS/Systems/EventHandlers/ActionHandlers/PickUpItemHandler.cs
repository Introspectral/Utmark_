using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;
using Utmark_ECS.Systems.EventSystem.EventType.ActionEvents;

namespace Utmark_ECS.Systems.EventHandlers.ActionHandlers
{
    public class PickUpItemHandler : ISystem
    {
        private InventorySystem _inventorySystem;
        private ComponentManager _componentManager;
        private EventManager _eventManager;
        public PickUpItemHandler(EventManager eventManager, ComponentManager componentManager, InventorySystem inventorySystem)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;
            _inventorySystem = inventorySystem;
            _eventManager.Subscribe<PickUpActionEventData>(OnPickUp);
        }

        private bool IsItem(Entities.Entity entity) =>
            _componentManager.GetComponentsForEntity(entity).Any(component => component is ItemComponent);

        private void OnPickUp(PickUpActionEventData data)
        {
            var picker = data.Picker;
            var itemName = _componentManager.GetComponent<NameComponent>(data.Item);
            var playerName = _componentManager.GetComponent<NameComponent>(picker);
            if (data.Item != null)
            {
                var item = data.Item;
                if (IsItem(item))
                {
                    _inventorySystem.AddItem(picker, item);
                    _eventManager.Publish(new MessageEventData(this, $"[color=green]*[/color] You picked up a [color=green]{itemName.Name}[/color]"));
                    _componentManager.RemoveComponent<PositionComponent>(item);
                }
                else
                {
                    _eventManager.Publish(new MessageEventData(this, $"[color=red]*[/color] You can not do that"));
                }
            }
            if (picker == null) { _eventManager.Publish(new MessageEventData(this, $"No Picker")); }
        }

        public void Draw(SpriteBatch spriteBatch) { }
        public void Update(GameTime gameTime) { }
    }
}
