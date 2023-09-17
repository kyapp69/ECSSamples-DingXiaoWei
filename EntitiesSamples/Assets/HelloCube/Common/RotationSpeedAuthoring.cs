using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace HelloCube
{
    // 旋转组件
    public class RotationSpeedAuthoring : MonoBehaviour
    {
        public float DegreesPerSecond = 360.0f;

        // 在烘焙过程中，此 Baker 将为实体子场景中的每个 RotationSpeedAuthoring 实例运行一次。
        // （嵌套创作组件的 Baker 类只是一个可选的样式问题。）
        class Baker : Baker<RotationSpeedAuthoring>
        {
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                // 这个实体将会移动
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // 将这个实体添加一个转速组件
                AddComponent(entity, new RotationSpeed
                {
                    RadiansPerSecond = math.radians(authoring.DegreesPerSecond) // 将角度转换为弧度   180°弧度是Π
                });
            }
        }
    }

    /// <summary>
    /// 定义一个旋转速度的的Entity  是一个数据结构的结构体
    /// </summary>
    public struct RotationSpeed : IComponentData
    {
        public float RadiansPerSecond;
    }
}
