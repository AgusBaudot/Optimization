using Sim.Foundation;

namespace Sim.Entities
{
    /// <summary>
    /// Holds the complete description of one entity's state at a single point in time.
    /// EntityRegistry stores a collection of them.
    /// </summary>
    public struct EntityData
    {
        public EntityId Id;
        public EntityType Type;
        public EntityState State;
        
        public SimVec2 Position;
        public float Rotation;

        public float Radius;
        public SimVec2 Velocity;

        public float Damage;
        public float Health; //Bullets have health = 1.
        public float DamageToTake;
    }
}