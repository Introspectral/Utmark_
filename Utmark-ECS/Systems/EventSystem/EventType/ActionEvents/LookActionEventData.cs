using Microsoft.Xna.Framework;
using Utmark_ECS.Entities;

namespace Utmark_ECS.Systems.EventSystem.EventType.ActionEvents
{
    public class LookActionEventData
    {
        public Entity Looker { get; }
        public List<Entity> Entities { get; }

        public Vector2 Position { get; }
        // Other necessary properties...

        public LookActionEventData(Entity looker, List<Entity> entities, Vector2 position)
        {
            Looker = looker;
            Entities = entities;
            Position = position;
        }
    }
}