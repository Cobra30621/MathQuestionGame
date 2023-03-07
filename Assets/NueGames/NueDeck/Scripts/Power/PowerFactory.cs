

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
    {
    public static class PowerFactory
    {
        private static readonly Dictionary<PowerType, PowerBase> PowerDict =
            new Dictionary<PowerType, PowerBase>();

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            // PowerDict.Clear();
            //
            // var allPowers = Assembly.GetAssembly(typeof(PowerBase)).GetTypes()
            //     .Where(t => typeof(PowerBase).IsAssignableFrom(t) && t.IsAbstract == false);
            //
            // foreach (var powerBase in allPowers)
            // {
            //     PowerBase power = Activator.CreateInstance(powerBase) as PowerBase;
            //     if (power != null) PowerDict.Add(power.PowerType, power);
            // }
            //
            // IsInitialized = true;
        }

        public static PowerBase GetPower(PowerType targetPower)
        {
            switch (targetPower)
            {
                case PowerType.Strength:
                    return new StrengthPower();
                case PowerType.Vulnerable:
                    return new VulnerablePower();
                case PowerType.Weak:
                    return new WeakPower();
                case PowerType.Stun:
                    return new StunPower();
                case PowerType.Block:
                    return new BlockPower();
                case PowerType.Dexterity:
                    return new DexterityPower();
                default:
                    Debug.LogError($"沒有 {targetPower} Power");
                    return null;
            }
        }
            

    }
}