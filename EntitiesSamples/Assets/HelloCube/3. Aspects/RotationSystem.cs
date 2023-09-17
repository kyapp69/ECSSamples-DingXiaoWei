using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HelloCube.Aspects
{
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.Aspects>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            double elapsedTime = SystemAPI.Time.ElapsedTime;  // 总帧时间

            // Rotate the cube directly without using the aspect.
            // The query matches all entities having the LocalTransform and RotationSpeed components.
            foreach (var (transform, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RadiansPerSecond * deltaTime);
            }

            // Rotate the cube using the aspect.
            // The query will include all components of VerticalMovementAspect.
            // Note that, unlike components, aspect type params of SystemAPI.Query are not wrapped in a RefRW or RefRO.
            foreach (var movement in SystemAPI.Query<VerticalMovementAspect>())  // 查找组合组件的实体
            {
                movement.Move(elapsedTime);
            }
        }
    }

    // 此方面的实例包装单个实体的 LocalTransform 和 RotationSpeed 组件。
    // （这个简单的例子对于方面来说可以说不是一个有价值的用例，但是更大的例子可以更好地展示它们的实用性。）
    // 什么是Aspects in DOTS: Aspect 将多个 componentData 组合起来使用，这样更接近面向对象的使用方法， 我们可以将挂载在同一个Entity上的多个component组合成一个Asepct，然后直接对Aspect进行修改
    readonly partial struct VerticalMovementAspect : IAspect
    {
        readonly RefRW<LocalTransform> m_Transform;
        readonly RefRO<RotationSpeed> m_Speed;

        public void Move(double elapsedTime)
        {
            m_Transform.ValueRW.Position.y = (float)math.sin(elapsedTime * m_Speed.ValueRO.RadiansPerSecond);
        }
    }
}
