using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Zombie
{
    public readonly partial struct ZombieEatAspect :IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<ZombieTimer> _zombieTimer;
        private readonly RefRO<ZombieEatProperties> _zombieEatProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float EatDamagePerSecond => _zombieEatProperties.ValueRO.EatDamagePerSecond;
        private float EatAmplitude => _zombieEatProperties.ValueRO.EatAmplitude;
        private float EatFrequency => _zombieEatProperties.ValueRO.EatFrequency;
        private float Heading => _heading.ValueRO.Value;
        private float ZombieTimer
        {
            get => _zombieTimer.ValueRO.Value;
            set => _zombieTimer.ValueRW.Value = value;
        }
        
        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEntity)
        {
            _zombieTimer.ValueRW.Value += deltaTime;
            var eatAngle = EatAmplitude * math.sin(EatFrequency * ZombieTimer);
            _transform.ValueRW.Rotation = quaternion.Euler(eatAngle, _heading.ValueRO.Value, 0);

            var eatDamage = EatDamagePerSecond * deltaTime;
            var curBrainDamage = new BrainDamageBufferElement {value = eatDamage};
            ecb.AppendToBuffer(sortKey, brainEntity, curBrainDamage);
        }
        
        public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, _transform.ValueRO.Position) <= brainRadiusSq - 1;
        }
    }
}