using Sim.Entities;

namespace Sim.Systems
{
    public interface ISimSystem
    {
        void Tick(float dt, EntityRegistry registry);
    }
}