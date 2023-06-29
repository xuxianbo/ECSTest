using Unity.Entities;

namespace TMG.Zombie
{
    [InternalBufferCapacity(8)]
    public struct BrainDamageBufferElement: IBufferElementData
    {
        public float value;
    }
}