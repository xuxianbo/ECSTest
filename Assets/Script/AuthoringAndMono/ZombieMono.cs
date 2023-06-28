using UnityEngine;
using Unity.Entities;

namespace TMG.Zombie
{
    public class ZombieMono: MonoBehaviour
    {
        public float RiseRate;
    }

    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new ZombieRiseRate
            {
                Value =  authoring.RiseRate
            });
            
        }
    }
}