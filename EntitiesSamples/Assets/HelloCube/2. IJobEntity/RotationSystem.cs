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
            state.RequireForUpdate<Execute.IJobEntity>(); // ����Update
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new RotateAndScaleJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = (float)SystemAPI.Time.ElapsedTime
            };
            job.Schedule();  // job��ʼִ��
        }
    }

    [BurstCompile]
    partial struct RotateAndScaleJob : IJobEntity
    {
        public float deltaTime;
        public float elapsedTime;

        // ��Դ�����У����� Execute() �Ĳ���������ѯ��
        // �������ѯ��ƥ����� LocalTransform��PostTransformMatrix �� RotationSpeed ���������ʵ�塣
        // ���ڳ����У���������ĳ߶Ȳ����ȣ�����ں決ʱ����һ��PostTransformMatrix�������
        // ref -> ���޸���Σ� in �����޸����
        void Execute(ref LocalTransform transform, ref PostTransformMatrix postTransform, in RotationSpeed speed) // ����Ҫ�� Ϊʲô��д�ڽӿ���
        {
            transform = transform.RotateY(speed.RadiansPerSecond * deltaTime);
            postTransform.Value = float4x4.Scale(1, math.sin(elapsedTime), 1);
        }
    }
}
