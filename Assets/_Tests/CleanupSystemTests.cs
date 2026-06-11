using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class CleanupSystemTests
    {
        [Test]
        public void Cleanup_OnDeath_FreesSlot()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Spawn entityA
            registry.TryCreateEntity(EntityType.Enemy, out var idA);
            
            //3. Retrieve it via the indexer, kill entityA, and save it back
            var entity = registry[idA];
            entity.State = EntityState.Dead;
            registry[idA] = entity;
            
            //4. Create the CleanupSystem
            ISimSystem cleanup = new CleanupSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 1.0f, registry);
            cleanup.Tick(dt: 1.0f, registry);

            //-------------------------ASSERT
            //6. Check !IsAlive
            Assert.AreEqual(EntityState.Unused, registry[idA].State);
        }

        [Test]
        public void Cleanup_OnDeath_FreesId()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Empty pool
            for (int i = 0; i < 127; i++)
            {
                registry.TryCreateEntity(EntityType.Enemy, out _);
            }
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            
            //3. Test pool
            Assert.AreEqual(false, registry.TryCreateEntity(EntityType.Enemy, out _));
            
            //4. Kill entity
            var entity = registry[id];
            entity.State = EntityState.Dead;
            registry[id] = entity;
            
            //5. Create the CleanupSystem
            ISimSystem cleanup = new CleanupSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 1.0f, registry);
            cleanup.Tick(dt: 1.0f, registry);

            //-------------------------ASSERT
            //7. Spawn another entity
            bool success = registry.TryCreateEntity(EntityType.Enemy, out _);
            
            //8. Check if their ID is valid?
            Assert.AreEqual(true, success);
        }
    }
}