using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// A static class for retrieving assets from specified directories.
    /// </summary>
    public static class AssetGetter 
    {
        public enum DataName
        {
            Card,
            EnemyEncounter,
            Character,
            Fx
        }

        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public static Dictionary<DataName, string> dataPaths = new Dictionary<DataName, string>()
        {
            { DataName.Card , "Assets/Data/Cards"},
            { DataName.EnemyEncounter , "Assets/Data/EnemyEncounter"},
            { DataName.Character, "Assets/Prefabs/Characters"},
            { DataName.Fx ,"Assets/Prefabs/FX"}
        };

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        /// <summary>
        /// Retrieves assets based on the given DataName.
        /// </summary>
        /// <param name="dataName">The type of data to retrieve assets for.</param>
        /// <returns>An IEnumerable of ValueDropdownItem containing the asset names and their corresponding objects.</returns>
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
        
        /// <summary>
        /// Retrieves assets from the specified root directory.
        /// </summary>
        /// <param name="root">The root directory to retrieve assets from.</param>
        /// <returns>An IEnumerable of ValueDropdownItem containing the asset names and their corresponding objects.</returns>
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