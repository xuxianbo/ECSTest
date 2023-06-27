using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.Zombie
{
    public class GraveyardMono : MonoBehaviour
    {
        /// <summary>
        /// 墓碑尺寸
        /// </summary>
        public float2 FieldDimensions;

        /// <summary>
        /// 生成墓碑的数量
        /// </summary>
        public int NumberTombstonesToSpawn;

        /// <summary>
        /// 墓碑预制件
        /// </summary>
        public GameObject TobstonePrefab;

        /// <summary>
        /// 随机种子
        /// </summary>
        public uint RandomSeed;


        public GameObject ZombiePrefab;
        public float ZombieSpawnRate;

        /// <summary>
        /// 烘焙
        /// </summary>
        class GraveyardMonoBaker : Baker<GraveyardMono>
        {
            public override void Bake(GraveyardMono authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new GraveyardProperties
                {
                    FieldDimensions = authoring.FieldDimensions,
                    NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
                    TobstonePrefab = GetEntity(authoring.TobstonePrefab, TransformUsageFlags.Dynamic),
                    ZombiePrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic),
                    ZombieSpawnRate = authoring.ZombieSpawnRate
                });

                AddComponent(entity, new GraveyardRandom
                {
                    Value = Random.CreateFromIndex(authoring.RandomSeed)
                });

                AddComponent<ZombieSpawnPoints>(entity);
                AddComponent<ZombieSpawnTimer>(entity);
            }
        }
    }
}