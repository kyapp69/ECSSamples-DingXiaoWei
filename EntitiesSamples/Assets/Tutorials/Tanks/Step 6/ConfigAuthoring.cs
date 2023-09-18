using Unity.Entities;
using UnityEngine;

namespace Tutorials.Tanks.Step6
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public GameObject TankPrefab;
        public int TankCount;
        public float SafeZoneRadius;

        class Baker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None); // 获取当前挂在的配置节点转成Entity
                AddComponent(entity, new Config // 添加上Config组件
                {
                    TankPrefab = GetEntity(authoring.TankPrefab, TransformUsageFlags.Dynamic),
                    TankCount = authoring.TankCount,
                    SafeZoneRadius = authoring.SafeZoneRadius
                });
            }
        }
    }

    public struct Config : IComponentData
    {
        public Entity TankPrefab;
        public int TankCount;
        public float SafeZoneRadius;   // Used in a later step.
    }
}
