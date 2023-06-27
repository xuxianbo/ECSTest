using Unity.Entities;
using Unity.Transforms;

namespace HelloCube
{
    readonly  partial struct RotationAspect:IAspect
    {
        readonly RefRW<LocalTransform> transform;
        readonly RefRO<RotationSpeed> rotationSpeed;

        public void Rotation(float deltaTime)
        {
            transform.ValueRW = transform.ValueRO.RotateY(rotationSpeed.ValueRO.RadiansPerSecond * deltaTime);
        }
    }
}