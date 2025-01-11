using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Relic.Data;
using UnityEngine;

namespace Relic
{
    public class RelicGenerator
    {
        private static IEnumerable<Type> RelicClasses;

        private static bool IsInitialized { get; set; }

        private static void Initialize()
        {
            RelicClasses = Assembly.GetAssembly(typeof(RelicBase)).GetTypes()
                .Where(t => typeof(RelicBase).IsAssignableFrom(t) && t.IsAbstract == false);
            
            IsInitialized = true;
        }

        /// <summary>
        ///  獲得能力(Relic)物件
        /// </summary>
        /// <param name="targetRelic"></param>
        /// <returns></returns>
        public static RelicBase GetRelic(RelicName targetRelic)
        {
            if(!IsInitialized)
                Initialize();
            
            string RelicBaseName = targetRelic.ToString() + "Relic";
            
            foreach (var RelicBase in RelicClasses)
            {
                if (RelicBase.Name == RelicBaseName)
                {
                    return  Activator.CreateInstance(RelicBase) as RelicBase;
                }
            }
            
            Debug.LogError($"沒有 Relic {RelicBaseName} 的 Class");
            return null;
        }
    }
}