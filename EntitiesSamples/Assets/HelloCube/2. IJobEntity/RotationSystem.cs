using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace HelloCube.JobEntity
{
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.IJobEntity>(); // 调用Update
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new RotateAndScaleJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = (float)SystemAPI.Time.ElapsedTime
            };
            job.Schedule();  // job开始执行
        }
    }

    [BurstCompile]
    partial struct RotateAndScaleJob : IJobEntity
    {
        public float deltaTime;
        public float elapsedTime;

        // 在源生成中，根据 Execute() 的参数创建查询。
        // 在这里，查询将匹配具有 LocalTransform、PostTransformMatrix 和 RotationSpeed 组件的所有实体。
        // （在场景中，根立方体的尺度不均匀，因此在烘焙时给它一个PostTransformMatrix组件。）
        // ref -> 可修改入参， in 不可修改入参
        void Execute(ref LocalTransform transform, ref PostTransformMatrix postTransform, in RotationSpeed speed) // 必须要有 为什么不写在接口里
        {
            transform = transform.RotateY(speed.RadiansPerSecond * deltaTime);
            postTransform.Value = float4x4.Scale(1, math.sin(elapsedTime), 1);
        }
    }
}
