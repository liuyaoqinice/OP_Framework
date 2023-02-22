using System;
using UnityEngine;

namespace OPFramework
{
    /// <summary>
    /// 框架主函数
    /// 仅做启动需要的环境
    /// </summary>
    public class Main: MonoBehaviour
    {
        private void Awake()
        {
            InitScreen();
        }

        // 初始化环境
        private void InitScreen()
        {
            Application.targetFrameRate = 60; // 设置渲染帧率
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 屏幕常量
        }
        
        // 初始化lua

        private void InitLua()
        {
            var time = Time.realtimeSinceStartup;
            if (LuaManager.lua == null)
                LuaManager.Init();
            
        }
        
    }
}