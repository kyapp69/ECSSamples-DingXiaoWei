using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Unity.Burst;

namespace HelloCube.GameObjectSync
{
    public partial struct DirectoryInitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // We need to wait for the scene to load before Updating, so we must RequireForUpdate at
            // least one component type loaded from the scene.

            state.RequireForUpdate<Execute.GameObjectSync>(); // ExecuteAuthoring中添加的组件
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;// 调用一次以后就关闭，防止重复创建cube

            var go = GameObject.Find("Directory");
            if (go == null)
            {
                throw new Exception("GameObject 'Directory' not found.");
            }

            var directory = go.GetComponent<Directory>(); // 获取 directory 游戏物体里面的Directory component

            var directoryManaged = new DirectoryManaged(); // 再添加组件
            directoryManaged.RotatorPrefab = directory.RotatorPrefab;
            directoryManaged.RotationToggle = directory.RotationToggle;

            var entity = state.EntityManager.CreateEntity();  // 创建了一个directoryManaged 单例 entity 用来同步数据
            state.EntityManager.AddComponentData(entity, directoryManaged);
        }
    }

    public class DirectoryManaged : IComponentData
    {
        public GameObject RotatorPrefab;
        public Toggle RotationToggle;

        // Every IComponentData class must have a no-arg constructor.
        public DirectoryManaged()
        {
        }
    }
}
