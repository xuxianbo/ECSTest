using UnityEngine;
using Unity.Entities;

namespace TMG.Zombie
{
    public class ZombieMono : MonoBehaviour
    {
        public float RiseRate;

        /// <summary>
        /// 行走速度
        /// </summary>
        public float WalkSpeed;
        /// <summary>
        /// 行走的幅度
        /// </summary>
        public float WalkAmplitude;
        /// <summary>
        /// 频率
        /// </summary>
        public float WalkFrequency;
        
        /// <summary>
        /// 每秒吃掉多上伤害
        /// </summary>
        public float EatDamagePerSecond;
        /// <summary>
        /// 吃得幅度
        /// </summary>
        public float EatAmplitude;
        /// <summary>
        /// 吃的频率
        /// </summary>
        public float EatFrequency;

    }

    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ZombieRiseRate
            {
                Value = authoring.RiseRate
            });

            AddComponent(entity, new ZombieWalkProperties
            {
                WalkSpeed = authoring.WalkSpeed,
                WalkAmplitude = authoring.WalkAmplitude,
                WalkFrequency = authoring.WalkFrequency
            });

            AddComponent(entity, new ZombieEatProperties()
            {
                EatDamagePerSecond = authoring.EatDamagePerSecond,
                EatAmplitude = authoring.EatAmplitude,
                EatFrequency = authoring.EatFrequency
            });

            AddComponent<ZombieTimer>(entity);
            AddComponent<ZombieHeading>(entity);
        }
    }
}