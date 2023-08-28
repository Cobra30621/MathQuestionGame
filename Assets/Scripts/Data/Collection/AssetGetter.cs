using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Collection
{
    /// <summary>
    /// 給 Odin 套件取得 Asset
    /// </summary>
    public static class AssetGetter 
    {
        public enum DataName
        {
            Card,
            EnemyEncounter,
            Character
        }

        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public static Dictionary<DataName, string> dataPaths = new Dictionary<DataName, string>()
        {
            { DataName.Card , "Assets/Data/Cards"},
            { DataName.EnemyEncounter , "Assets/Data/EnemyEncounter"},
            { DataName.Character, "Assets/Prefabs/Characters"}
        };

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        public static IEnumerable GetAssets(DataName dataName)
        {
            if (dataPaths.TryGetValue(dataName, out var root))
            {
                return GetAssets(dataPaths[dataName]);
            }
            else
            {
                Debug.LogError($"{nameof(dataPaths)} doesn't contain {dataName}");
                return null;
            }
        }
        
        public static IEnumerable GetAssets(string root)
        {
            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith(root))
                .Select(x => x.Substring(root.Length))
                .Select(x =>
                    new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(root + x)));
        }
#endif
    }
}