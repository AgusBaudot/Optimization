using System;
using Sim.Entities;
using Sim.Foundation;
using Sim.Systems;

namespace Sim.Engine
{
    public class SimulationEngine
    {
        public FrameSnapshot Front => _front;
        
        private EntityRegistry _registry;
        private ISimSystem[] _systems;
        private FrameSnapshot _back;
        private FrameSnapshot _front;
        
        public SimulationEngine(EntityRegistry entityRegistry, ISimSystem[] systems)
        {
            _registry = entityRegistry;
            _systems = systems;
            _back = new FrameSnapshot();
            _front = new FrameSnapshot();
        }

        public void Tick(float dt)
        {
            foreach (var system in _systems)
                system.Tick(dt, _registry);

            _back.EntityCount = 0;
            for (int i = 0; i < _registry.Entities.Length; i++)
            {
                EntityView snapshotEntity = _back.Entities[i];
                EntityData entityData = _registry.Entities[i];

                snapshotEntity.EntityId = entityData.Id;
                snapshotEntity.Position = entityData.Position;
                snapshotEntity.Type = entityData.Type;
                snapshotEntity.State = entityData.State;
                snapshotEntity.Radius = entityData.Radius;

                if (entityData.Type == EntityType.Player)
                {
                    _back.PlayerHealth = entityData.Health / (float)EntityStats.BASE_PLAYER_HEALTH;
                }

                if (entityData.State == EntityState.Alive)
                {
                    _back.EntityCount++;
                }
                
                _back.Entities[i] = snapshotEntity;
            }

            _back.CurrentWave = _registry.CurrentWave;
            
            (_front, _back) = (_back, _front);
        }
    }
}