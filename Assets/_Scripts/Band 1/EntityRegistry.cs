using System;
using System.Collections.Generic;
using Sim.Foundation;

namespace Sim.Entities
{
    public class EntityRegistry
    {
        private const int ENTITY_REGISTRY_SIZE = 128;
        
        public EntityData[] Entities => _entityDatas;
        public int CurrentWave => _currentWave;
        
        private readonly EntityData[] _entityDatas = new EntityData[ENTITY_REGISTRY_SIZE];
        
        private readonly Queue<int> _entityIds = new(ENTITY_REGISTRY_SIZE); //Holds the free IDs
        private readonly int[] _entityGenerations = new int[ENTITY_REGISTRY_SIZE]; //Holds each current generation for each ID.

        private int _currentWave;

        public EntityData this[EntityId id]
        {
            get
            {
                if (_entityGenerations[id.ID] != id.Generation)
                    throw new InvalidOperationException($"Attempted to read stale EntityId {id.ID}");
                
                return _entityDatas[id.ID];
            }

            set
            {
                if (_entityGenerations[id.ID] != id.Generation)
                    return;
                
                _entityDatas[id.ID] = value;
            }
        }
        
        //Ctor
        public EntityRegistry()
        {
            for (int i = 0; i < ENTITY_REGISTRY_SIZE; i++)
            {
                _entityIds.Enqueue(i);
            }
        }
        
        //Create an entity.
        public bool TryCreateEntity(EntityType type, out EntityId entityId)
        {
            //Handle finding a free ID, making the EntityID, Creating the EntityData, storing it in the dictionary.
            //Construct the EntityID.
            bool success = TryCreateEntityId(out entityId);
            
            if (!success)
                return false;
            
            //Create the EntityData
            EntityData entity = new EntityData
            {
                Id = entityId,
                Type = type,
                State = EntityState.Alive,
                Position = SimVec2.zero,
                Rotation = 0,
                Radius = 0,
                Velocity = SimVec2.zero,
                Damage = 0,
                Health = 0
            };

            //Store created entity
            _entityDatas[entityId.ID] = entity;

            return true;
        }

        private bool TryCreateEntityId(out EntityId entityId)
        {
            entityId = new EntityId();
            if (_entityIds.Count == 0)
            {
                return false;
            }
            int id = _entityIds.Dequeue();
            int gen = _entityGenerations[id] += 1;
            //Since it's a struct, it initializes a value type on the stack, generating 0 garbage.
            entityId = new EntityId(id, gen);
            return true;
        }
        
        //Destroying an entity
        public bool DestroyEntity(EntityId id)
        {
            _entityDatas[id.ID].State = EntityState.Unused;
            _entityIds.Enqueue(id.ID);
            return true;
        }
        
        public void SetCurrentWave(int wave)
        {
            _currentWave = wave;
        }
    }
}