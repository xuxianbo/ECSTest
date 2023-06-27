using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Zombie
{
    public struct GraveyardRandom:IComponentData
    {
        public Random Value;
    }
}