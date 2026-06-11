using Sim.Foundation;

namespace Sim.Engine
{
    public class FrameSnapshot
    {
        public EntityView[] Entities = new EntityView[128];
        public int EntityCount;
        public int CurrentWave;
        public float PlayerHealth;
    }
}