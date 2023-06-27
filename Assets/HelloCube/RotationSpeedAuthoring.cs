using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace HelloCube
{
    public class RotationSpeedAuthoring : MonoBehaviour
    {

        public float DegreesPerSecond = 360.0f;

        // In baking, this Baker will run once for every RotationSpeedAuthoring instance in an entity subscene.
        // (Nesting an authoring component's Baker class is simply an optional matter of style.)
        class Baker : Baker<RotationSpeedAuthoring>
        {
            private float2 test = float2.zero;
            
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                // float2.
                // The entity will be moved
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new RotationSpeed
                {
                    RadiansPerSecond = math.radians(authoring.DegreesPerSecond)
                });
            }
        }
    }


    public struct RotationSpeed : IComponentData
    {
        public float RadiansPerSecond;
    }
}