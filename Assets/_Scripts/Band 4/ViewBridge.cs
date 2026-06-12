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
        private readonly Queue<GameObject> _bulletViews = new(15);
        private readonly Queue<GameObject> _enemyViews = new(50);
        
        //1. Read engine.Front
        //2. For each EntityView in the snapshot, if state == alive, ensure a GameObject exists for it. If state != alive, ensure no GO is visible.
        //3. For active GameObjects, set transform.position form EntityView.Position
        public ViewBridge(SimulationEngine engine, GameObject playerPrefab, GameObject bulletPrefab, GameObject enemyPrefab)
        {
            _engine = engine;
            _playerPrefab = playerPrefab;
            _bulletPrefab = bulletPrefab;
            _enemyPrefab = enemyPrefab;

            for (int i = 0; i < 50; i++)
            {
                if (i < 15)
                {
                    var bullet = Object.Instantiate(_bulletPrefab);
                    bullet.name = $"{EntityType.Bullet}_{i}";
                    bullet.SetActive(false);
                    _bulletViews.Enqueue(bullet);
                }

                var enemy = Object.Instantiate(enemyPrefab);
                enemy.name = $"{EntityType.Enemy}_{i}";
                enemy.SetActive(false);
                _enemyViews.Enqueue(enemy);
            }
        }

        public void Render()
        {
            var currentSnapshot = _engine.Front;
            
            foreach (EntityView entity in currentSnapshot.Entities)
            {
                //Cache lookup once per frame
                bool viewExists = _activeViews.TryGetValue(entity.EntityId, out var entityView);

                if (entity.State == EntityState.Alive)
                {
                    if (!viewExists)
                    {
                        Spawn(entity); // Not in dict? Spawn it.
                    }
                    else
                    {
                        if (entityView == null)
                        {
                            _activeViews.Remove(entity.EntityId);
                            continue;
                        }

                        // Already in dict? Update it.
                        Vector3 position = new Vector3(entity.Position.x, entity.Position.y, 0f);
                        entityView.transform.position = position;
                    }
                }
                else if (viewExists)
                {
                    // Dead, but still in dict? Despawn it.
                    Despawn(entity);
                }
            }
        }

        private void Spawn(EntityView entityView)
        {
            GameObject spawnedView = null;

            switch (entityView.Type)
            {
                case EntityType.Player:
                    spawnedView = Object.Instantiate(_playerPrefab);
                    spawnedView.name = "Player_View";
                    break;

                case EntityType.Bullet:
                    spawnedView = _bulletViews.Dequeue();
                    break;

                case EntityType.Enemy:
                    spawnedView = _enemyViews.Dequeue();
                    break;
            }

            if (spawnedView != null)
            {
                _activeViews.TryAdd(entityView.EntityId, spawnedView);

                Vector3 position = new Vector3(entityView.Position.x, entityView.Position.y, 0f);
                spawnedView.transform.position = position;
                spawnedView.SetActive(true);
            }
        }

        private void Despawn(EntityView entityView)
        {
            if (!_activeViews.TryGetValue(entityView.EntityId, out var viewToDespawn))
                return;

            viewToDespawn.SetActive(false);

            switch (entityView.Type)
            {
                case EntityType.Player:
                    Object.Destroy(viewToDespawn);
                    break;

                case EntityType.Bullet:
                    _bulletViews.Enqueue(viewToDespawn);
                    break;

                case EntityType.Enemy:
                    _enemyViews.Enqueue(viewToDespawn);
                    break;
            }

            _activeViews.Remove(entityView.EntityId);
        }
    }
}