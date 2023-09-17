using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace HelloCube
{
    // ��ת���
    public class RotationSpeedAuthoring : MonoBehaviour
    {
        public float DegreesPerSecond = 360.0f;

        // �ں決�����У��� Baker ��Ϊʵ���ӳ����е�ÿ�� RotationSpeedAuthoring ʵ������һ�Ρ�
        // ��Ƕ�״�������� Baker ��ֻ��һ����ѡ����ʽ���⡣��
        class Baker : Baker<RotationSpeedAuthoring>
        {
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                // ���ʵ�彫���ƶ�
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // �����ʵ�����һ��ת�����
                AddComponent(entity, new RotationSpeed
                {
                    RadiansPerSecond = math.radians(authoring.DegreesPerSecond) // ���Ƕ�ת��Ϊ����   180�㻡���Ǧ�
                });
            }
        }
    }

    /// <summary>
    /// ����һ����ת�ٶȵĵ�Entity  ��һ�����ݽṹ�Ľṹ��
    /// </summary>
    public struct RotationSpeed : IComponentData
    {
        public float RadiansPerSecond;
    }
}
