using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace TMG.Zombie
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem: ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();

            new ZombieRiseJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct ZombieRiseJob:IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        private void Execute(ZombieRiseAspect zombieRiseAspect, [EntityIndexInQuery]int sortKey)
        {
            zombieRiseAspect.Rise(DeltaTime);
            if (zombieRiseAspect.IsAboveGround)
            {
                
            }
        }
    }
}