using System;
using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class EnemyAISystem : ISimSystem
    {
        public void Tick(float dt, EntityRegistry registry)
        {
            bool playerFound = false;
            SimVec2 playerPos = new SimVec2(0, 0);

            foreach (EntityData entity in registry.Entities)
            {
                if (entity.State == EntityState.Alive && entity.Type == EntityType.Player)
                {
                    playerPos = entity.Position;
                    playerFound = true;
                    break;
                }
            }

            foreach (EntityData entity in registry.Entities)
            {
                if (entity.State != EntityState.Alive || entity.Type != EntityType.Enemy)
                    continue;

                var copy = entity;

                if (!playerFound)
                {
                    copy.Velocity = new SimVec2(0, 0);
                    registry[copy.Id] = copy;
                    continue;
                }

                float dx = playerPos.x - copy.Position.x;
                float dy = playerPos.y - copy.Position.y;

                float distance = MathF.Sqrt(dx * dx + dy * dy);

                if (distance > 0.01f)
                {
                    float dirX = dx / distance;
                    float dirY = dy / distance;

                    copy.Velocity = new SimVec2(dirX * EntityStats.BASE_ENEMY_SPEED, dirY * EntityStats.BASE_ENEMY_SPEED);
                }
                else
                {
                    copy.Velocity = new SimVec2(0, 0);
                }

                registry[copy.Id] = copy;
            }
        }
    }
}