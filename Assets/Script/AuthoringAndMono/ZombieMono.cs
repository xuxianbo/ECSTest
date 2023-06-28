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


            AddComponent<ZombieTimer>(entity);
            AddComponent<ZombieHeading>(entity);

        }
    }
}