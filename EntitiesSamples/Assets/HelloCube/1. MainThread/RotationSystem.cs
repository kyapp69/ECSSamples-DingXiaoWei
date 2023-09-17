using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace HelloCube.MainThread
{
    public partial struct RotationSystem : ISystem  // ����ִ��
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Ϊ��ǰ��ystem�����һ�����������ǵ�ǰworld�����ٴ���һ��Execute.MainThread component attached with an entity,��ǰsystem�Ż����update
            state.RequireForUpdate<Execute.MainThread>(); // ���ϵͳ��Ҫִ��Update��ִ�в�����MainThread�����ݽṹ��Entity
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // ѭ���������� LocalTransform ����� RotationSpeed �����ÿ��ʵ�塣
            // ��ÿ�ε����У�transform ���ᱻ����һ���� LocalTransform �Ķ�д���ã�
            // �ٶȱ������ RotationSpeed �����ֻ�����á�
            foreach (var (transform, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>()) // �������д���RotationSpeed�����ʵ�壬��ҪAddComponent��ӵ�
            {
                // ValueRW �� ValueRO ������ʵ�����ֵ�����á�
                // �������� ValueRW �Զ�д���ʽ��а�ȫ��飬��
                // ValueRO ��ֻ�����ʽ��а�ȫ��顣
                transform.ValueRW = transform.ValueRO.RotateY( // ����Y����ת
                    speed.ValueRO.RadiansPerSecond * deltaTime);
            }
        }
    }
}
