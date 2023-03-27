

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
    {
    public static class PowerFactory
    {
     
        private static IEnumerable<Type> PowerClasses;

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            PowerClasses = Assembly.GetAssembly(typeof(PowerBase)).GetTypes()
                .Where(t => typeof(PowerBase).IsAssignableFrom(t) && t.IsAbstract == false);
            
            IsInitialized = true;
        }

        public static PowerBase GetPower(PowerType targetPower)
        {
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