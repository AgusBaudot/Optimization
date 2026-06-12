using Sim.Entities;
using Sim.Foundation;

namespace Sim.Systems
{
    public class PlayerControlSystem : ISimSystem
    {
        private readonly InputContainer _inputContainer;
        private float _cooldownRemaining;
        
        public PlayerControlSystem(InputContainer container)
        {
            _inputContainer = container;
            _cooldownRemaining = EntityStats.BASE_PLAYER_COOLDOWN;
        }
        
        public void Tick(float dt, EntityRegistry registry)
        {
            EntityData player = default;
            player.State = EntityState.Dead;
            foreach (var entity in registry.Entities)
            {
                if (entity.State != EntityState.Alive || entity.Type != EntityType.Player)
                {
                    continue;
                }

                player = entity;
                player.Velocity = _inputContainer.Input.MovementInput * EntityStats.BASE_PLAYER_SPEED;
                registry[entity.Id] =  player;
            }

            if (EntityState.Dead == player.State)
                return;
            
            _cooldownRemaining -= dt;
            if (_cooldownRemaining <= 0f)
            {
                _cooldownRemaining = EntityStats.BASE_PLAYER_COOLDOWN;
                Fire(registry, player.Position);
            }
        }

        private void Fire(EntityRegistry registry, SimVec2 playerPosition)
        {
            registry.TryCreateEntity(EntityType.Bullet, out var bullet);
            EntityData bulletData = registry[bullet];
            SimVec2 direction = (_inputContainer.Input.MousePosition - playerPosition).normalized;
            if (direction.Equals(SimVec2.zero))
            {
                direction = SimVec2.right;
            }
                
            bulletData.Velocity = direction * EntityStats.BASE_BULLET_SPEED;
            bulletData.Position = playerPosition;
            bulletData.Health = 1;
            bulletData.Radius = 0.5f;
            bulletData.Damage = EntityStats.BASE_BULLET_DAMAGE;
            registry[bullet] = bulletData;
        }
    }
}