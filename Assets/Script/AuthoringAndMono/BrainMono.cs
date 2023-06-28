using System.Threading;
using UnityEngine;
using Unity.Entities;
namespace TMG.Zombie
{
    public class BrainMono:MonoBehaviour
    {
        public float BrainHealth;
    }

    public class BrainBaker : Baker<BrainMono>
    {
        public override void Bake(BrainMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<BrainTag>(entity);
        }
    }
}