using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 特效(FX)的GameObject資料
    /// </summary>
    [CreateAssetMenu(fileName = "FX Data", menuName = "NueDeck/Containers/FX Data", order = 2)]
    public class FXData : SerializedScriptableObject
    {

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<FxName, GameObject> fxDict;
        
        
        public GameObject GetFX(FxName fxName)
        {
            if (fxDict.ContainsKey(fxName))
            {
                return fxDict[fxName];
            }
            else
            {
                Debug.LogError($"fxDict 沒有 {fxName} 的特效，請去設定");
                return null;
            }
        }
        
    }
    
}