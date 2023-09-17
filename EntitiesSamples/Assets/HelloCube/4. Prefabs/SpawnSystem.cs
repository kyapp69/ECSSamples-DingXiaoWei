using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HelloCube.Prefabs
{
    public partial struct SpawnSystem : ISystem
    {
        uint updateCounter;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // This call makes the system not update unless at least one entity in the world exists that has the Spawner component.
            state.RequireForUpdate<Spawner>();

            state.RequireForUpdate<Execute.Prefabs>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 创建一个匹配所有具有 RotationSpeed 组件的实体的查询。
            // （查询在源生成中缓存，因此不会产生每次更新时重新创建它的成本。）
            var spinningCubesQuery = SystemAPI.QueryBuilder().WithAll<RotationSpeed>().Build();

            // Only spawn cubes when no cubes currently exist.
            if (spinningCubesQuery.IsEmpty)
            {
                var prefab = SystemAPI.GetSingleton<Spawner>().Prefab;

                // Instantiating an entity creates copy entities with the same component types and values.
                var instances = state.EntityManager.Instantiate(prefab, 500, Allocator.Temp);

                // 与 new Random() 不同，CreateFromIndex() 对随机种子进行哈希处理
                // 这样相似的种子就不会产生相似的结果
                var random = Random.CreateFromIndex(updateCounter++);

                foreach (var entity in instances)
                {
                    // Update the entity's LocalTransform component with the new position.
                    var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                    transform.ValueRW.Position = (random.NextFloat3() - new float3(0.5f, 0, 0.5f)) * 20;
                }
            }
        }
    }
}
