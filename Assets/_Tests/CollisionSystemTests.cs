using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class CollisionSystemTests
    {
        [Test]
        public void CollisionSystem_EnemyBullet_Overlapping()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Enemy, out var enemy);
            registry.TryCreateEntity(EntityType.Bullet, out var bullet);
            
            //3. Set up their radius and dmg
            var enemyCopy = registry[enemy];
            enemyCopy.Radius = 0.1f;
            registry[enemy] = enemyCopy;
            
            var bulletCopy = registry[bullet];
            bulletCopy.Radius = 0.1f;
            bulletCopy.Damage = 2;
            registry[bullet] = bulletCopy;
            
            //4. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Check if correct entity detected collision
            Assert.AreEqual(registry[bullet].Damage, registry[enemy].DamageToTake);
            Assert.AreEqual(0, registry[bullet].DamageToTake);
        }
        
        [Test]
        public void CollisionSystem_EnemyPlayer_Overlapping()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Enemy, out var enemy);
            registry.TryCreateEntity(EntityType.Player, out var player);
            
            //3. Set up their radius and dmg
            var enemyCopy = registry[enemy];
            enemyCopy.Radius = 0.1f;
            enemyCopy.Damage = 2;
            registry[enemy] = enemyCopy;
            
            var playerCopy = registry[player];
            playerCopy.Radius = 0.1f;
            registry[player] = playerCopy;
            
            //4. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //5. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //6. Check if correct entity detected collision
            Assert.AreEqual(registry[enemy].Damage, registry[player].DamageToTake);
            Assert.AreEqual(0, registry[enemy].DamageToTake);
        }

        [Test]
        public void CollisionSystem_EnemyBullet_NotOverlapping()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Enemy, out var enemy);
            registry.TryCreateEntity(EntityType.Bullet, out var bullet);
            
            //3. Set up their radius
            var enemyCopy = registry[enemy];
            enemyCopy.Radius = 3;
            enemyCopy.Damage = 1;
            registry[enemy] = enemyCopy;
            
            var bulletCopy = registry[bullet];
            bulletCopy.Radius = 2;
            bulletCopy.Damage = 2;
            
            //4. Move EntityB out fo the way
            bulletCopy.Position = new SimVec2(6, 0);
            registry[bullet] = bulletCopy;
            
            //5. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //7. Check if no collision was detected.
            Assert.AreEqual(0, registry[bullet].DamageToTake);
            Assert.AreEqual(0, registry[enemy].DamageToTake);
        }

        [Test]
        public void CollisionSystem_EnemyBullet_Touching()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Enemy, out var enemy);
            registry.TryCreateEntity(EntityType.Bullet, out var bullet);
            
            //3. Set up their radius and damage
            var enemyCopy = registry[enemy];
            enemyCopy.Radius = 3;
            registry[enemy] = enemyCopy;
            
            var bulletCopy = registry[bullet];
            bulletCopy.Radius = 2;
            bulletCopy.Damage = 1;
            
            //4. Move EntityB just far enough
            bulletCopy.Position = new SimVec2(5, 0);
            registry[bullet] = bulletCopy;
            
            //5. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //7. Check if correct entity detected collision
            Assert.AreEqual(registry[bullet].Damage, registry[enemy].DamageToTake);
            Assert.AreEqual(0, registry[bullet].DamageToTake);
        }

        [Test]
        public void CollisionSystem_TwoBullets_NoCollision()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Bullet, out var bulletA);
            registry.TryCreateEntity(EntityType.Bullet, out var bulletB);
            
            //3. Set up their radius
            var enemyCopy = registry[bulletA];
            enemyCopy.Radius = 3;
            enemyCopy.Damage = 1;
            registry[bulletA] = enemyCopy;
            
            var bulletCopy = registry[bulletB];
            bulletCopy.Radius = 2;
            bulletCopy.Damage = 2;
            
            //4. Move EntityB just far enough
            bulletCopy.Position = new SimVec2(5, 0);
            registry[bulletB] = bulletCopy;
            
            //5. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //7. Check if no collision was detected
            Assert.AreEqual(0, registry[bulletA].DamageToTake);
            Assert.AreEqual(0, registry[bulletB].DamageToTake);
        }
        
        [Test]
        public void CollisionSystem_TwoEnemies_NoCollision()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Enemy, out var enemyA);
            registry.TryCreateEntity(EntityType.Enemy, out var enemyB);
            
            //3. Set up their radius
            var enemyCopy = registry[enemyA];
            enemyCopy.Radius = 3;
            enemyCopy.Damage = 1;
            registry[enemyA] = enemyCopy;
            
            var bulletCopy = registry[enemyB];
            bulletCopy.Radius = 2;
            bulletCopy.Damage = 2;
            
            //4. Move EntityB just far enough
            bulletCopy.Position = new SimVec2(5, 0);
            registry[enemyB] = bulletCopy;
            
            //5. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //7. Check if no collision was detected
            Assert.AreEqual(0, registry[enemyA].DamageToTake);
            Assert.AreEqual(0, registry[enemyB].DamageToTake);
        }
        
        [Test]
        public void CollisionSystem_PLayerBullet_NoCollision()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new EntityRegistry();
            
            //2. Create EntityA & Entity B
            registry.TryCreateEntity(EntityType.Player, out var player);
            registry.TryCreateEntity(EntityType.Bullet, out var bullet);
            
            //3. Set up their radius
            var enemyCopy = registry[player];
            enemyCopy.Radius = 3;
            enemyCopy.Damage = 1;
            registry[player] = enemyCopy;
            
            var bulletCopy = registry[bullet];
            bulletCopy.Radius = 2;
            bulletCopy.Damage = 2;
            
            //4. Move EntityB just far enough
            bulletCopy.Position = new SimVec2(5, 0);
            registry[bullet] = bulletCopy;
            
            //5. Create the CollisionSystem.
            ISimSystem system = new CollisionSystem();

            //-------------------------ACT
            //6. Call system.Tick(dt: 2.0f, registry);
            system.Tick(dt: 2.0f, registry);

            //-------------------------ASSERT
            //7. Check if no collision was detected
            Assert.AreEqual(0, registry[player].DamageToTake);
            Assert.AreEqual(0, registry[bullet].DamageToTake);
        }
        
        [Test]
        public void CollisionSystem_TwoBullets_AccumulateDamage()
        {
            EntityRegistry registry = new EntityRegistry();

            registry.TryCreateEntity(EntityType.Enemy, out var enemy);
            registry.TryCreateEntity(EntityType.Bullet, out var bulletA);
            registry.TryCreateEntity(EntityType.Bullet, out var bulletB);

            var e = registry[enemy]; e.Radius = 5f; registry[enemy] = e;

            var bA = registry[bulletA]; bA.Radius = 1f; bA.Damage = 10; registry[bulletA] = bA;
            var bB = registry[bulletB]; bB.Radius = 1f; bB.Damage = 15; registry[bulletB] = bB;

            ISimSystem system = new CollisionSystem();
            system.Tick(1f, registry);

            Assert.AreEqual(25, registry[enemy].DamageToTake);
        }
    }
}