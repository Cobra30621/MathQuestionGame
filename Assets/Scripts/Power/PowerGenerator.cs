using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power {
    /// <summary>
    /// 產生能力(Power)物件
    /// </summary>
    public static class PowerGenerator
    {
        private static IEnumerable<Type> PowerClasses;

        private static bool IsInitialized { get; set; }

        private static void Initialize()
        {
            PowerClasses = Assembly.GetAssembly(typeof(PowerBase)).GetTypes()
                .Where(t => typeof(PowerBase).IsAssignableFrom(t) && t.IsAbstract == false);
            
            IsInitialized = true;
        }

        /// <summary>
        ///  獲得能力(Power)物件
        /// </summary>
        /// <param name="targetPower"></param>
        /// <returns></returns>
        public static PowerBase GetPower(PowerType targetPower)
        {
            if(!IsInitialized)
                Initialize();
            
            string powerBaseName = targetPower.ToString() + "Power";
            
            foreach (var powerBase in PowerClasses)
            {
                if (powerBase.Name == powerBaseName)
                {
                    return  Activator.CreateInstance(powerBase) as PowerBase;
                }
            }
            
            Debug.LogError($"沒有 Power {powerBaseName} 的 Class");
            return null;
        }
    }
}