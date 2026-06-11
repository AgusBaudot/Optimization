using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class MovementSystemTests
    {
        [Test]
        public void Tick_WithVelocity_UpdatesPosition()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            //2. Spawn an entity using TryCreateEntity.
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            //3. Retrieve it via the indexer, set Velocity to (1, 0), Position to (0, 0), and save it back.
            var enemy = registry[id];
            enemy.Position = SimVec2.zero;
            enemy.Velocity = SimVec2.right;
            registry[id] = enemy;
            //4. Create the MovementSystem.
            ISimSystem movementSystem = new MovementSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            movementSystem.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Retrieve the entity from the registry again.
            enemy = registry[id];

            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(new SimVec2(2f, 0f), enemy.Position);
        }

        [Test]
        public void Tick_IfDead_SkipsEntity()
        {
            
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Spawn an entity using TryCreateEntity.
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            
            //3. Retrieve it via the indexer, set Velocity to (1, 0), Position to (0, 0), and save it back.
            var enemy = registry[id];
            enemy.State = EntityState.Dead;
            registry[id] = enemy;
            
            //4. Create the MovementSystem.
            ISimSystem movementSystem = new MovementSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            movementSystem.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Retrieve the entity from the registry again.
            enemy = registry[id];

            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(SimVec2.zero, enemy.Position);
        }
    }
}