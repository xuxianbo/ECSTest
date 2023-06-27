using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.Zombie
{
    /// <summary>
    /// 墓地
    /// </summary>
    public struct GraveyardProperties : IComponentData
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
        public Entity TobstonePrefab;

        /// <summary>
        /// 僵尸预制件
        /// </summary>
        public Entity ZombiePrefab;

        /// <summary>
        /// 
        /// </summary>
        public float ZombieSpawnRate;
    }

    /// <summary>
    /// 定时器
    /// </summary>
    public struct ZombieSpawnTimer : IComponentData
    {
        public float Value;
    }
}