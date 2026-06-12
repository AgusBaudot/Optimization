using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class CombatSystem : ISimSystem
    {
        public void Tick(float dt, EntityRegistry registry)
        {
            foreach (EntityData entity in registry.Entities)
            {
                if (entity.State != EntityState.Alive || entity.DamageToTake == 0)
                {
                    continue;
                }
                
                EntityData entityData = entity;
                entityData.Health -= entityData.DamageToTake;
                entityData.DamageToTake = 0;

                if (entityData.Health <= 0)
                {
                    entityData.Health = 0;
                    entityData.State = EntityState.Dead;
                    if (entityData.Type == EntityType.Player)
                    {
                        //Lose
                    }
                }
                
                registry[entity.Id] = entityData;
            }
        }
    }
}