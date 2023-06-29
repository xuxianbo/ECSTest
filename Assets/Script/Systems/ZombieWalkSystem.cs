using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.Zombie
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieRiseSystem))]
    public partial struct ZombieWalkSystem : ISystem
    {
        [BurstCompile]
        void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ZombieWalkProperties>();
        }
        [BurstCompile]
        void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        void OnUpdate(ref SystemState state)
        {

            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainLocalTransform = SystemAPI.GetComponent<LocalTransform>(brainEntity);
            var brainRadius = brainLocalTransform.Scale * 5f + 0.5f;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ZombieWalkJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                BrainRadiusSq = brainRadius,
                Position = brainLocalTransform.Position,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()

            }.ScheduleParallel();
        }

    }
    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float BrainRadiusSq;
        public float DeltaTime;
        public Unity.Mathematics.float3 Position;

        public EntityCommandBuffer.ParallelWriter ECB;
        [BurstCompile]
        private void Execute(ZombieRiseAspect zombieRiseAspect, [EntityIndexInQuery] int sortKey)
        {
            zombieRiseAspect.Walk(this.DeltaTime);

            if (zombieRiseAspect.IsInStoppingRange(Position, BrainRadiusSq))
            {
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombieRiseAspect.Entity, false);
                ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombieRiseAspect.Entity, true);
                // ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombieRiseAspect.Entity, false);
            }
        }
    }
}