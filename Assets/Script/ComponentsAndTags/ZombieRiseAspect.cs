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

        public void Rise(float deltaTime)
        {
            _transform.ValueRW.Position += math.up() * _zombieRiseRate.ValueRO.Value * deltaTime;
        }

        public bool IsAboveGround => _transform.ValueRO.Position.y >= 0f;
    }
}