using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class MovementSystem : ISimSystem
    {
        public void Tick(float dt, EntityRegistry registry)
        {
            foreach (EntityData entity in registry.Entities)
            {
                if (entity.State != EntityState.Alive)
                    continue;
                
                //Fetch entity
                EntityData entityData = entity;
                //Calculate new Position
                entityData.Position += entity.Velocity * dt;
                //Write new position with indexer
                registry[entity.Id] = entityData;
            }
        }
    }
}