using Sim.Entities;
using Sim.Foundation;
using UnityEngine;

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
                        Debug.LogError("Clean this up");
                        Application.Quit();
                    }
                }
                
                registry[entity.Id] = entityData;
            }
        }
    }
}