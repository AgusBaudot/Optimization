using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class CombatSystemTests
    {
        [Test]
        public void CombatSystem_AppliesDamage()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Spawn an entity using TryCreateEntity.
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            
            //3. Retrieve it via the indexer, set Health and DamageToTake, and save it back
            var enemy = registry[id];
            enemy.Health = 100;
            enemy.DamageToTake = 40;
            registry[id] = enemy;
            
            //4. Create the CombatSystem.
            ISimSystem system = new CombatSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Retrieve the entity from the registry again.
            enemy = registry[id];

            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(0, enemy.DamageToTake);
            Assert.AreEqual(60, enemy.Health);
        }

        [Test]
        public void CombatSystem_ChangesState_OnDeath()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Spawn an entity using TryCreateEntity.
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            
            //3. Retrieve it via the indexer, set Health and DamageToTake, and save it back
            var enemy = registry[id];
            enemy.Health = 100;
            enemy.DamageToTake = 140;
            registry[id] = enemy;
            
            //4. Create the CombatSystem.
            ISimSystem system = new CombatSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Retrieve the entity from the registry again.
            enemy = registry[id];

            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(0, enemy.DamageToTake);
            Assert.AreEqual(0, enemy.Health);
            Assert.AreEqual(EntityState.Dead, enemy.State);
        }
        
        [Test]
        public void COmbatSystem_DoesNothing_NoDamageToTake()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Spawn an entity using TryCreateEntity.
            registry.TryCreateEntity(EntityType.Enemy, out var id);
            
            //3. Retrieve it via the indexer, set Health and DamageToTake, and save it back
            var enemy = registry[id];
            enemy.Health = 100;
            registry[id] = enemy;
            
            //4. Create the CombatSystem.
            ISimSystem system = new CombatSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Retrieve the entity from the registry again.
            enemy = registry[id];

            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(0, enemy.DamageToTake);
            Assert.AreEqual(100, enemy.Health);
            Assert.AreEqual(EntityState.Alive, enemy.State);
        }
        
        [Test]
        public void CombatSystem_SkipsDead_WithPendingDamage()
        {
            EntityRegistry registry = new EntityRegistry();
            registry.TryCreateEntity(EntityType.Enemy, out var id);

            var e = registry[id];
            e.Health = 50;
            e.DamageToTake = 30;
            e.State = EntityState.Dead;
            registry[id] = e;

            ISimSystem system = new CombatSystem();
            system.Tick(1f, registry);

            Assert.AreEqual(EntityState.Dead, registry[id].State);
            Assert.AreEqual(50, registry[id].Health);
        }
    }
}