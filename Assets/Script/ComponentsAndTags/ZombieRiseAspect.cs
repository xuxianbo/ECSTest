using System.IO.Compression;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Zombie
{
    public readonly partial struct ZombieRiseAspect:IAspect
    {
        public readonly Entity Entity;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<ZombieRiseRate> _zombieRiseRate;
        private readonly RefRO<ZombieWalkProperties> _zombieWalkProperties;
        private readonly RefRW<ZombieTimer> _walkTimer;
        private readonly RefRO<ZombieHeading> _heading;


        private float WalkSpeed => _zombieWalkProperties.ValueRO.WalkSpeed;
        private float WalkAmplitude => _zombieWalkProperties.ValueRO.WalkAmplitude;
        private float WalkFrequency => _zombieWalkProperties.ValueRO.WalkFrequency;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        public void Rise(float deltaTime)
        {
            _transform.ValueRW.Position += math.up() * _zombieRiseRate.ValueRO.Value * deltaTime;
        }

        public void Walk(float deltaTime)
        {
            /// <summary>
            /// 行走
            /// </summary>
            WalkTimer += deltaTime;
            _transform.ValueRW.Position += _transform.ValueRW.Forward() * _zombieWalkProperties.ValueRO.WalkSpeed * deltaTime;
            
            /// <summary>
            /// 左右摇摆
            /// </summary>
            /// <param name="WalkTimer"></param>
            /// <returns></returns>
            var angle = WalkAmplitude *math.sin(WalkFrequency * WalkTimer);
            _transform.ValueRW.Rotation = quaternion.Euler(0, Heading, angle);
        }

        public bool IsInStoppingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, _transform.ValueRO.Position) <= brainRadiusSq;
        }

        public bool IsAboveGround => _transform.ValueRO.Position.y >= 0f;


        /// <summary>
        /// 设置在地面上
        /// </summary>
        public void SetAtGroundLevel()
        {
            var position = _transform.ValueRO.Position;
            position.y = 0;
            _transform.ValueRW.Position = position;
        }
    }
}