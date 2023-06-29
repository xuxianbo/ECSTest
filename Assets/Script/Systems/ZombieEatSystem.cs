using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.Zombie
{
    [BurstCompile]
    public partial struct ZombieEatSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BrainTag>();
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();

            var localTransform = SystemAPI.GetComponent<LocalTransform>(brainEntity);
            
            var brainScale = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale;
            var brainRadius = brainScale * 5f + 1f;
            
            new ZombieEatJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                BrainEntity = brainEntity,
                BrainRadiusSq =  brainRadius*brainRadius,
                Position = localTransform.Position
            }.ScheduleParallel();
        }
        
    }
    
    [BurstCompile]
    public partial struct ZombieEatJob:IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity BrainEntity;
        public float BrainRadiusSq;
        public Unity.Mathematics.float3 Position;
        [BurstCompile]
        public void Execute(ZombieEatAspect zombie, [EntityIndexInChunk]int sortKey)
        {
            if (zombie.IsInEatingRange(Position, BrainRadiusSq))
            {
                zombie.Eat(DeltaTime, ECB, sortKey, BrainEntity);
            }
            else
            {
                // ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.Entity, false);
                // ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, true);
            }
        }
    }
}