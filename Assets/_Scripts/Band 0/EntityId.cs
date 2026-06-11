using System;

namespace Sim.Foundation
{
    ///<summary>
    ///Unique identification for every entity.
    ///</summary>
    public struct EntityId : IEquatable<EntityId>
    {
        public int ID { get; }
        public int Generation { get; }

        //internal constructor in only callable from within the same assembly. Other bands don't know it exists.
        internal EntityId(int id, int gen)
        {
            (ID, Generation) = (id, gen);
        }

        public override bool Equals(object obj)
        {
            return obj is EntityId e && Equals(e);
        }

        public bool Equals(EntityId other)
        {
            return ID == other.ID && Generation == other.Generation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Generation);
        }
    }
}