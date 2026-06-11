using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class CleanupSystem : ISimSystem
    {
        public void Tick(float dt, EntityRegistry registry)
        {
            //Iterate through all entities
            foreach (var entity in registry.Entities)
            {
                //If an entity's state is dead, destroy it.
                if (entity.State == EntityState.Dead)
                {
                    registry.DestroyEntity(entity.Id);
                }
            }
        }
    }
}