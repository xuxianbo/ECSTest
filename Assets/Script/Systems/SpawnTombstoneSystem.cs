using HelloCube;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Zombie
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperties>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 当运行一次之后关闭
            state.Enabled = false;
            
            var entity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
            var graveyardAspect = SystemAPI.GetAspect<GraveyardAspect>(entity);
            var spawnPoints = new NativeList<float3>(Allocator.Temp);
            var tombstoneOffset = new float3(0f,-2f,1f);
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            for (int i = 0; i < graveyardAspect.NumberTombstonesToSpawn; i++)
            {
                var newTombstone = ecb.Instantiate(graveyardAspect.TombstonePrefab);
                var newTombstoneTransform = graveyardAspect.GetRandomTombstoneTransform();
                ecb.SetComponent(newTombstone, newTombstoneTransform);
                spawnPoints.Add(newTombstoneTransform.Position + tombstoneOffset);
            }

            // graveyardAspect.ZombieSpawnPoints = spawnPoints.ToArray(Allocator.Persistent);
            ecb.Playback(state.EntityManager);
        }
    }
}
