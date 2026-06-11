using Sim.Engine;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;
using UnityEngine;

namespace Sim.View
{
    public class GameBootstrap
    {
        private static GameObject _playerPrefab, _bulletPrefab, _enemyPrefab;

        private static InputContainer _container;

        private static SimulationEngine _engine;
        private static InputReader _inputReader;
        private static ViewBridge _viewBridge;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            //Unity Asset Load
            _playerPrefab = Resources.Load<GameObject>("Player");
            _bulletPrefab = Resources.Load<GameObject>("Bullet");
            _enemyPrefab = Resources.Load<GameObject>("Enemy");

            //Band 0
            _container = new InputContainer();

            //Band 1
            var registry = new EntityRegistry();

            if (registry.TryCreateEntity(EntityType.Player, out var playerId))
            {
                var playerEntity = registry[playerId];
                playerEntity.Position = new SimVec2(0, 0);
                playerEntity.Health = EntityStats.BASE_PLAYER_HEALTH;
                registry[playerId] = playerEntity;
            }

            var waves = new WaveConfig[]
            {
                new WaveConfig(0, 10, 6, new WaveEndInfo(WaveFinishConditionType.KillBased, 1), 1, 1),
                new WaveConfig(1, 0, 6, new WaveEndInfo(WaveFinishConditionType.TimeBased, 5), 1, 1),
            };

            //Band 2
            var waveSystem = new WaveSystem(waves);
            waveSystem.OnAllWavesCompleted += Win;

            var systems = new ISimSystem[]
            {
                new CleanupSystem(),
                new PlayerControlSystem(_container),
                new EnemyAISystem(),
                new MovementSystem(),
                new CollisionSystem(),
                new CombatSystem(),
                waveSystem
            };

            //Band 3
            _engine = new SimulationEngine(registry, systems);

            //Band 4
            //If I pass engine.Tick(dt) to the player loop injector, it's solely gonna call that, whereas it should call InputReader.Calculate(InputContainer container) -> Engine.Tick(float dt) -> ViewBridge.Render();
            var loopInjector = new PlayerLoopInjector(() => RunGame());
            _viewBridge = new ViewBridge(_engine, _playerPrefab, _bulletPrefab, _enemyPrefab);
            _inputReader = new InputReader();

            //How and where should I call InputReader, SimulationEngine.Tick, and ViewBridge.Render();
        }

        private static void RunGame()
        {
            if (!Application.isPlaying)
                return;

            _inputReader.CalculateInput(_container);

            _engine.Tick(Time.deltaTime);

            _viewBridge.Render();
        }

        private static void Win()
        {
            Debug.Log("Win");
        }
    }
}