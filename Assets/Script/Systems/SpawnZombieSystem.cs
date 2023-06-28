using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TMG.Zombie
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SpawnTombstoneSystem))]
    public partial struct SpawnZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ZombieSpawnTimer>();
        }

        [BurstCompile]
        void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Debug.Log("============>>>SpawnZombieSystem OnUpdate");
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            var job = new SpawnZombieJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            };

            state.Dependency = job.Schedule(state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        [BurstCompile]
        private void Execute(GraveyardAspect graveyard)
        {
            // Debug.Log("============>>>SpawnZombieSystem Execute");

            graveyard.ZombieSpawnTimer -= DeltaTime;
            if (!graveyard.TimeToSpawnZombie) return;
            // if(!graveyard.ZombieSpawnPointInitialized()) return;

            graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
            var newZombie = ECB.Instantiate(graveyard.ZombiePrefab);

            var newZombieTransform = graveyard.GetZombieSpawnPoint();
            ECB.SetComponent(newZombie, newZombieTransform);

            var zombieHeading = MathHelpers.GetHeading(newZombieTransform.Position, graveyard.Position);
            ECB.SetComponent(newZombie, new ZombieHeading{Value = zombieHeading});
        }
    }

}