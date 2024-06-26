﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utmark_ECS.Components;
using Utmark_ECS.Intefaces;
using Utmark_ECS.Managers;
using Utmark_ECS.Systems.EventSystem.EventType;

namespace Utmark_ECS.Systems.EventHandlers
{
    public class CollisionHandler : ISystem
    {
        private readonly EventManager _eventManager;
        private readonly ComponentManager _componentManager;

        public CollisionHandler(EventManager eventManager, ComponentManager componentManager)
        {
            _eventManager = eventManager;
            _componentManager = componentManager;

            _eventManager.Subscribe<CollisionEventData>(OnCollision);
        }

        private void OnCollision(CollisionEventData data)
        {
            if (data is CollisionEventData collisionData)
            {
                HandleCollision(collisionData);
            }
        }

        private void HandleCollision(CollisionEventData collisionData)
        {
            var entityBComponents = _componentManager.GetComponentsForEntity(collisionData.EntityB);


            foreach (var component in entityBComponents)
            {
                if (component is ItemComponent item)
                {
                    HandleItemCollision(collisionData, item);

                }
                else if (component is NameComponent name)
                {
                    HandleNameCollision(collisionData, name);
                }
            }
        }

        private void HandleItemCollision(CollisionEventData collisionData, ItemComponent item)
        {
            // Handle collision with ItemComponent
            //_eventManager.Publish(new MessageEventData(this, $"[color=blue]-=[/color] You come across something, it seems to be a {item.ItemType} [color=blue]=-[/color]"));

        }

        private void HandleNameCollision(CollisionEventData collisionData, NameComponent name)
        {
            _eventManager.Publish(new MessageEventData(this, $"[color=orange]*[/color] There is something on the ground here."));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

