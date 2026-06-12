using Sim.Entities;
using Sim.Foundation;
using System.Collections.Generic;

namespace Sim.Systems
{
    public class BulletLifetimeSystem : ISimSystem
    {
        private readonly Dictionary<EntityId, float> _bulletsLifetime = new();

        public void Tick(float dt, EntityRegistry registry)
        {
            foreach (EntityData entity in registry.Entities)
            {
                if (entity.Type == EntityType.Bullet)
                {
                    //Cache lookup once per frame
                    bool viewExists = _bulletsLifetime.TryGetValue(entity.Id, out var entityView);

                    if (entity.State == EntityState.Alive)
                    {
                        //Not in dict? Add it.
                        if (!viewExists)
                        {
                            _bulletsLifetime[entity.Id] = EntityStats.BASE_BULLET_LIFETIME;
                        }
                        else
                        {
                            // Already in dict? Update it.
                            _bulletsLifetime[entity.Id] -= dt;

                            if (entityView <= 0)
                            {
                                var bullet = entity;
                                bullet.State = EntityState.Dead;
                                registry[entity.Id] = bullet;
                                _bulletsLifetime.Remove(entity.Id);
                            }
                        }
                    }
                    else if (viewExists)
                    {
                        // Dead, but still in dict? Despawn it.
                        var bullet = entity;
                        bullet.State = EntityState.Dead;
                        registry[entity.Id] = bullet;
                        _bulletsLifetime.Remove(entity.Id);
                    }
                }
            }
        }
    }
}