using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Zombie
{
    public struct ZombieSpawnPoints:IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsBlob> Value;
    }
    
    public struct ZombieSpawnPointsBlob
    {
        public BlobArray<float3> Value;
    }
}