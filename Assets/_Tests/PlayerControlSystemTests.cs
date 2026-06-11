using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class PlayerControlSystemTests
    {
        [Test]
        public void PlayerControlSystem_MovesPlayer_WhenInput()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create player
            registry.TryCreateEntity(EntityType.Player, out var player);
            
            //3. Create a new Input Container
            var container = new InputContainer
            {
                Input = new InputFrame(new SimVec2(3, 2), SimVec2.zero)
            };

            //4. Create the PlayerControlSystem
            ISimSystem system = new PlayerControlSystem(container);

            //-------------------------ACT
            //5. Call system.Tick(dt: 1.0f, registry);
            system.Tick(dt: 1.0f, registry);

            //-------------------------ASSERT
            //6. Check if their ID is valid?
            Assert.AreEqual(new SimVec2(3, 2), registry[player].Velocity);
        }

        [Test]
        public void PlayerControlSystem_SpawnsBullet_AfterCooldown()
        {
            EntityRegistry registry = new EntityRegistry();
            registry.TryCreateEntity(EntityType.Player, out var player);

            var container = new InputContainer
            {
                Input = new InputFrame(SimVec2.zero, SimVec2.up)
            };
            ISimSystem system = new PlayerControlSystem(container);
            system.Tick(10f, registry);

            bool bulletFound = false;
            foreach (var entity in registry.Entities)
            {
                if (entity.State == EntityState.Alive && entity.Type == EntityType.Bullet)
                {
                    bulletFound = true;
                    break;
                }
            }
            Assert.IsTrue(bulletFound);
        }
    }
}