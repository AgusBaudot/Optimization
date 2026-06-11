using NUnit.Framework;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Tests
{
    [TestFixture]
    public class WaveSystemTests
    {
        [Test]
        //Wave starts spawn correct amount of enemies after time.
        public void WaveSystem_OnWaveStart_SpawnsEnemies()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new();

            //2. Populate with valid wave info
            WaveConfig waveConfig = new WaveConfig(
                0,
                3,
                10,
                new WaveEndInfo(WaveFinishConditionType.KillBased, 1),
                1,
                1
            );

            //3. Create the WaveSystem.
            ISimSystem system = new WaveSystem(new[] { waveConfig });

            //-------------------------ACT
            //4. Call system.Tick()
            system.Tick(dt: 10f, registry);

            //-------------------------ASSERT
            //5. Check the result with Assert.AreEqual(expectedValue, actualValue);
            int enemyCount = 0;
            foreach (var entity in registry.Entities)
            {
                if (entity.Type == EntityType.Enemy && entity.State == EntityState.Alive)
                    enemyCount++;
            }

            Assert.AreEqual(3, enemyCount);
        }

        [Test]
        //Wave ends correctly for both reasons.
        //Update _currentWave correctly.
        public void WaveSystem_EndsWave_KillBased()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new();

            //2. Populate with valid wave info
            WaveConfig wave1 = new WaveConfig(
                0,
                3,
                10,
                new WaveEndInfo(WaveFinishConditionType.KillBased, 1),
                1,
                1
            );
            WaveConfig wave2 = new WaveConfig(
                1,
                0,
                0,
                new WaveEndInfo(WaveFinishConditionType.TimeBased, 2),
                1,
                1
            );

            //3. Create the WaveSystem.
            ISimSystem system = new WaveSystem(new[] { wave1, wave2 });

            //-------------------------ACT
            //4. Let wave spawn enemies
            system.Tick(dt: 1f, registry);
            
            //5. Kill all enemies
            foreach (var entity in registry.Entities)
            {
                if (entity.State == EntityState.Unused && EntityType.Enemy != entity.Type)
                    continue;
                
                var data = entity;
                data.State = EntityState.Dead;
                registry[data.Id] = data;
            }
            
            //6. Tick system to update wave
            system.Tick(0.5f, registry);

            //-------------------------ASSERT
            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(((WaveSystem)system).CurrentWave, 1);
        }

        [Test]
        public void WaveSystem_EndsWave_TimeBased()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new();

            //2. Populate with valid wave info
            WaveConfig wave1 = new WaveConfig(
                0,
                0,
                10,
                new WaveEndInfo(WaveFinishConditionType.TimeBased, 2),
                1,
                1
            );
            WaveConfig wave2 = new WaveConfig(
                1,
                0,
                0,
                new WaveEndInfo(WaveFinishConditionType.TimeBased, 2),
                1,
                1
            );

            //3. Create the WaveSystem.
            ISimSystem system = new WaveSystem(new[] { wave1, wave2 });

            //-------------------------ACT
            //4. Call system.Tick()
            system.Tick(dt: 0.1f, registry);
            system.Tick(dt: 1f, registry);

            //-------------------------ASSERT
            //5. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(0, ((WaveSystem)system).CurrentWave);
            
            //6. Let enough time pass for wave to finish
            system.Tick(dt: 1f, registry);
            
            //7. Check again.
            Assert.AreEqual(1, ((WaveSystem)system).CurrentWave);
        }

        [Test]
        //When all waves complete, win
        public void WaveSystem_OnAllWavesCompleted_Win()
        {
            //-------------------------ARRANGE
            //1. Create the EntityRegistry
            EntityRegistry registry = new();

            //2. Populate with valid wave info
            WaveConfig wave1 = new WaveConfig(
                0,
                3,
                10,
                new WaveEndInfo(WaveFinishConditionType.KillBased, 1),
                1,
                1
            );

            //3. Create the WaveSystem.
            ISimSystem system = new WaveSystem(new[] { wave1 });

            //-------------------------ACT
            //4. Subscribe to win event
            bool win = false;
            ((WaveSystem)system).OnAllWavesCompleted += () =>
            {
                win = true;
            };
            
            //5. Let wave spawn enemies
            system.Tick(dt: 1f, registry);
            
            //5. Kill all enemies
            foreach (var entity in registry.Entities)
            {
                var data = entity;
                data.State = EntityState.Dead;
                registry[data.Id] = data;
            }
            
            //6. Tick system to update wave
            system.Tick(0.5f, registry);

            //-------------------------ASSERT
            //7. Check the result with Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(true, win);
        }
    }
}