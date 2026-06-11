using Sim.Foundation;

namespace Sim.Engine
{
    public struct EntityView
    {
        public EntityId EntityId;
        public SimVec2 Position;
        public EntityType Type;
        public EntityState State;
        public float Radius;
    }
}