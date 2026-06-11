using System;
using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class WaveSystem : ISimSystem
    {
        private const float SPAWN_DELAY = 0.05f;
        
        public int CurrentWave => _currentWave;

        public event Action OnAllWavesCompleted;
        
        private readonly WaveConfig[] _totalWaves;
        private int _currentWave;
        private bool _spawningPhase;
        private float _waveTimeElapsed;
        private float _spawnTimeElapsed;
        private int _enemiesRemainingToSpawn;

        public WaveSystem(WaveConfig[] configs)
        {
            _totalWaves = configs;
            _spawningPhase = true;
            _enemiesRemainingToSpawn = configs[0].EnemyAmount;
        }
        
        public void Tick(float dt, EntityRegistry registry)
        {
            var currentWave = _totalWaves[_currentWave];
            
            if (_spawningPhase)
            {
                _spawnTimeElapsed += dt;

                while (_spawnTimeElapsed >= SPAWN_DELAY && _enemiesRemainingToSpawn > 0)
                {
                    _spawnTimeElapsed -= SPAWN_DELAY;

                    int currentIndex = currentWave.EnemyAmount - _enemiesRemainingToSpawn;

                    SpawnEnemy(registry, currentIndex, currentWave.EnemyAmount);
                    _enemiesRemainingToSpawn--;
                }

                if (_enemiesRemainingToSpawn <= 0)
                {
                    _spawningPhase = false;
                }
                
                return;
            }
            
            //Check for time
            if (currentWave.EndInfo.FinishCondition == WaveFinishConditionType.TimeBased)
            {
                _waveTimeElapsed += dt;

                if (_waveTimeElapsed >= currentWave.EndInfo.Amount)
                {
                    TriggerWaveEnd(registry);
                }
            }
            //Check for kill count
            else
            {
                float enemiesRemaining = 0;
                foreach (EntityData entity in registry.Entities)
                {
                    if (entity.State != EntityState.Alive || entity.Type != EntityType.Enemy)
                        continue;

                    enemiesRemaining++;
                }
                //Wave must end if % of enemies are killed.
                if ((currentWave.EnemyAmount - enemiesRemaining) / currentWave.EnemyAmount >= currentWave.EndInfo.Amount)
                {
                    TriggerWaveEnd(registry);
                }
            }
        }

        private void SpawnEnemy(EntityRegistry registry, int currentIndex, int totalEnemies)
        {
            registry.TryCreateEntity(EntityType.Enemy, out var enemyId);
            var enemy = registry[enemyId];

            float angle = (currentIndex / (float)totalEnemies) * (MathF.PI * 2f);

            float x = MathF.Cos(angle) * _totalWaves[_currentWave].Radius;
            float y = MathF.Sin(angle) * _totalWaves[_currentWave].Radius;

            enemy.Position = new SimVec2(x, y);

            enemy.Health = EntityStats.BASE_ENEMY_HEALTH * _totalWaves[_currentWave].HealthScaling;
            enemy.Damage = EntityStats.BASE_ENEMY_DAMAGE * _totalWaves[_currentWave].DamageScaling;
            registry[enemyId] = enemy;
        }

        private void TriggerWaveEnd(EntityRegistry registry)
        {
            if (_totalWaves.Length - 1 == _currentWave)
            {
                OnAllWavesCompleted?.Invoke();
                return;
            }

            _waveTimeElapsed = 0;
            _currentWave++;
            _spawningPhase = true;
            registry.SetCurrentWave(_currentWave);
            _enemiesRemainingToSpawn = _totalWaves[_currentWave].EnemyAmount;
        }
    }
}