using Sim.Engine;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;
using UnityEngine;

namespace Sim.View
{
    public class GameBootstrap
    {
        public GameBootstrap()
        {
            var registry = new EntityRegistry();
            var input = new InputContainer();
            var systems = new ISimSystem[]
            {
                new CleanupSystem(),
                new PlayerControlSystem(input),
                new MovementSystem(),
                new CollisionSystem(),
                new CombatSystem(),
                new WaveSystem(waves)
            };
            var engine = new SimulationEngine(registry, systems);
        }
    }
}