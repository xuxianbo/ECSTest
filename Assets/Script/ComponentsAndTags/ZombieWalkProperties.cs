using Unity.Entities;
namespace TMG.Zombie
{
    public struct ZombieWalkProperties : IComponentData, IEnableableComponent
    {
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


    public struct ZombieEatProperties : IComponentData, IEnableableComponent
    {

    }

    /// <summary>
    /// 行走时间
    /// </summary>
    public struct ZombieTimer : IComponentData
    {
        public float Value;
    }

    /// <summary>
    /// 僵尸标记
    /// </summary>
    public struct ZombieHeading : IComponentData
    {
        public float Value;
    }

    /// <summary>
    /// 新僵尸标记
    /// </summary>
    public struct NewZombieTag : IComponentData { }
}