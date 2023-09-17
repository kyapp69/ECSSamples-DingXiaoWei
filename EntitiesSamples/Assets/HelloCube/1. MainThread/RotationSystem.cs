using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace HelloCube.MainThread
{
    public partial struct RotationSystem : ISystem  // 反射执行
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // 为当前的ystem添加了一个条件，除非当前world中至少存在一个Execute.MainThread component attached with an entity,当前system才会调用update
            state.RequireForUpdate<Execute.MainThread>(); // 这个系统需要执行Update，执行参数是MainThread的数据结构的Entity
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // 循环遍历具有 LocalTransform 组件和 RotationSpeed 组件的每个实体。
            // 在每次迭代中，transform 都会被分配一个对 LocalTransform 的读写引用，
            // 速度被分配给 RotationSpeed 组件的只读引用。
            foreach (var (transform, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>()) // 遍历所有带有RotationSpeed组件的实体，需要AddComponent添加的
            {
                // ValueRW 和 ValueRO 都返回实际组件值的引用。
                // 区别在于 ValueRW 对读写访问进行安全检查，而
                // ValueRO 对只读访问进行安全检查。
                transform.ValueRW = transform.ValueRO.RotateY( // 绕着Y轴旋转
                    speed.ValueRO.RadiansPerSecond * deltaTime);
            }
        }
    }
}
