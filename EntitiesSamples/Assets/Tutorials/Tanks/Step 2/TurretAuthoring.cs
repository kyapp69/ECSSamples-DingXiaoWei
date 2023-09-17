using Unity.Entities;
using UnityEngine;

namespace Tutorials.Tanks.Step2
{
    // Authoring MonoBehaviours are regular GameObject components.
    // They constitute the inputs for the baking systems which generates ECS data.
    class TurretAuthoring : MonoBehaviour
    {
        // Bakers convert authoring MonoBehaviours into entities and components.
        public GameObject CannonBallPrefab;
        public Transform CannonBallSpawn;

        class Baker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                // GetEntity returns the baked Entity form of a GameObject.
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Turret
                {
                    CannonBallPrefab = GetEntity(authoring.CannonBallPrefab, TransformUsageFlags.Dynamic),
                    CannonBallSpawn = GetEntity(authoring.CannonBallSpawn, TransformUsageFlags.Dynamic)
                });

                AddComponent<Shooting>(entity);
            }
        }
    }

    public struct Turret : IComponentData
    {
        // These fields will be used in step 4.


        // This entity will reference the prefab to be instantiated when the cannon shoots.
        public Entity CannonBallPrefab;

        // This entity will reference the nozzle of the cannon, where cannon balls should be spawned.
        public Entity CannonBallSpawn;
    }

    // 该组件将在步骤 8 中使用。
    // 这是一个标签组件，也是一个“启用组件”。
    // 可以打开和关闭此类组件，而无需从实体中删除组件，
    // 这会降低效率并且不会保留组件的值。
    // Enableable 组件最初被启用。
    public struct Shooting : IComponentData, IEnableableComponent
    {
    }
}
