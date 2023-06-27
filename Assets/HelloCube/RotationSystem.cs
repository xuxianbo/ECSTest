using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace HelloCube
{
    public partial struct RotationSystem:ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<HelloCube.RotationSpeed>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<RotationSpeed>();
            var rotationAspect = SystemAPI.GetAspect<RotationAspect>(entity);
            
            float deltaTime = SystemAPI.Time.DeltaTime;
            rotationAspect.Rotation(deltaTime);
        }

    }
}