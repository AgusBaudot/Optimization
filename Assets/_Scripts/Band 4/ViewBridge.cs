using System.Collections.Generic;
using Sim.Engine;
using Sim.Foundation;
using UnityEngine;

namespace Sim.View
{
    public class ViewBridge
    {
        private SimulationEngine _engine;
        private GameObject _playerPrefab;
        private GameObject _bulletPrefab;
        private GameObject _enemyPrefab;

        private readonly Dictionary<EntityId, GameObject> _activeViews = new();
        private readonly Queue<GameObject> _poolViews = new(128);
        
        //1. Read engine.Front
        //2. For each EntityView in the snapshot, if state == alive, ensure a GameObject exists for it. If state != alive, ensure no GO is visible.
        //3. For active GameObjects, set transform.position form EntityView.Position
        public ViewBridge(SimulationEngine engine, GameObject playerPrefab, GameObject bulletPrefab, GameObject enemyPrefab)
        {
            _engine = engine;
            _playerPrefab = playerPrefab;
            _bulletPrefab = bulletPrefab;
            _enemyPrefab = enemyPrefab;

            for (int i = 0; i < 128; i++)
            {
                _poolViews.Enqueue(new GameObject(i.ToString()));
            }
        }

        public void Render()
        {
            var currentSnapshot = _engine.Front;
            
            foreach (EntityView entity in currentSnapshot.Entities)
            {
                //If snapshot has entity that's alive and not in dictionary, Spawn.
                if (entity.State == EntityState.Alive && !_activeViews.ContainsKey(entity.EntityId))
                {
                    Spawn(entity);
                }
                
                //If snapshot has entity that's not alive and in dictionary, Died.
                if ((entity.State == EntityState.Dead || entity.State == EntityState.Unused) && _activeViews.ContainsKey(entity.EntityId))
                {
                    Despawn(entity);
                }
                
                //If snapshot has entity that's alive and in dict, Update.
                if (entity.State == EntityState.Alive && _activeViews.TryGetValue(entity.EntityId, out var entityView))
                {
                    Vector3 position = new Vector3(entity.Position.x, entity.Position.y);
                    entityView.transform.position = position;
                }
            }
        }

        private void Spawn(EntityView entityView)
        {
            //Spawn should:
            //1. Register new entry in dict
            //2. Get from pool.
            _activeViews.TryAdd(entityView.EntityId, _poolViews.Dequeue());
            //3. Initialize accordingly (set sprite and position)
            Vector3 position = new Vector3(entityView.Position.x, entityView.Position.y);
            _activeViews[entityView.EntityId].transform.position = position;
            _activeViews[entityView.EntityId].SetActive(true);
            //Set sprite according to entity type
            //4. Enjoy!
        }

        private void Despawn(EntityView entityView)
        {
            //Despawn should:
            //1. Kill visual entity (set to false).
            _activeViews[entityView.EntityId].SetActive(false);
            //2. Return it to pool.
            _poolViews.Enqueue(_activeViews[entityView.EntityId]);
            //3. Remove from dict.
            _activeViews.Remove(entityView.EntityId);
            //4. Enjoy!
        }
    }
}