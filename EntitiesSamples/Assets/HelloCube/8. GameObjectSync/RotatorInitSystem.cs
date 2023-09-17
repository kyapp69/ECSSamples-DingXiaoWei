using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace HelloCube.GameObjectSync
{
    [UpdateInGroup(typeof(InitializationSystemGroup))] // 指定system在哪个分组之下
    public partial struct RotatorInitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DirectoryManaged>();
            state.RequireForUpdate<Execute.GameObjectSync>();
        }

        // This OnUpdate accesses managed objects, so it cannot be burst compiled.
        public void OnUpdate(ref SystemState state)
        {
            var directory = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();
            var ecb = new EntityCommandBuffer(Allocator.Temp); // https://zhuanlan.zhihu.com/p/328218005 结合Burst编译提高性能

            // Instantiate the associated GameObject from the prefab.
            // 查询subscene中存在RotationSpeed 并且没有 RotatorGO 也就是无 prefab实体 的 entity
            // Query只能得到Component 加上 WithEntityAccess() 可以访问对应的 entity 
            foreach (var (goPrefab, entity) in
                     SystemAPI.Query<RotationSpeed>()
                         .WithNone<RotatorGO>()
                         .WithEntityAccess())
            {
                var go = GameObject.Instantiate(directory.RotatorPrefab);

                // We can't add components to entities as we iterate over them, so we defer the change with an ECB.
                ecb.AddComponent(entity, new RotatorGO(go));
            }

            ecb.Playback(state.EntityManager);
        }
    }

    public class RotatorGO : IComponentData
    {
        public GameObject Value;

        public RotatorGO(GameObject value)
        {
            Value = value;
        }

        // Every IComponentData class must have a no-arg constructor.
        public RotatorGO()
        {
        }
    }
}

