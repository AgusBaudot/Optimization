using System;
using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    /// <summary>
    /// Nested loop runs at n^2. With 128 entities at worst (max cap), that is
    /// 16384 comparisons, each being a subtraction and a multiplication.
    /// </summary>
    public class CollisionSystem : ISimSystem
    {
        public void Tick(float dt, EntityRegistry registry)
        {
            for (int i = 0; i < registry.Entities.Length - 1; i++)
            {
                for (int j = i + 1; j < registry.Entities.Length; j++)
                {
                    Process(registry.Entities[i], registry.Entities[j], registry);
                }
            }
        }

        private void Process(EntityData a, EntityData b, EntityRegistry registry)
        {
            if (a.State != EntityState.Alive ||
                b.State != EntityState.Alive)
            {
                return;
            }

            if (!CollidablePair(a, b))
            {
                return;
            }
                    
            if (!(SimVec2.SquaredDistance(a.Position, b.Position) <=
                  (a.Radius + b.Radius) *
                  (a.Radius + b.Radius)))
            {
                return;
            }
                    
            var damagedEntity = ShouldTakeDamage(a, b);

            if (damagedEntity.damages.Type == EntityType.Bullet)
            {
                damagedEntity.damages.DamageToTake += damagedEntity.damaged.Damage;
                registry[damagedEntity.damages.Id] = damagedEntity.damages;
            }

            damagedEntity.damaged.DamageToTake += damagedEntity.damages.Damage;
            registry[damagedEntity.damaged.Id] = damagedEntity.damaged;
        }

        private bool CollidablePair(EntityData a, EntityData b)
        {
            return a.Type != b.Type && (a.Type != EntityType.Player || b.Type != EntityType.Bullet) && (a.Type != EntityType.Bullet || b.Type != EntityType.Player);
        }

        private (EntityData damaged, EntityData damages) ShouldTakeDamage(EntityData a, EntityData b)
        {
            if (a.Type == EntityType.Enemy)
            {
                if (b.Type == EntityType.Bullet)
                    return (a, b);
                return (b, a);
            }
            if (b.Type == EntityType.Enemy)
            {
                if (a.Type == EntityType.Bullet)
                    return (b, a);
                return (a, b);
            }

            throw new InvalidOperationException(
                $"Attempted collision between non-collidable entity types: {a.Type}, {b.Type}");
        }
    }
}