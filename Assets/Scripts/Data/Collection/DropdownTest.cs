using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Card.Data;
using NueGames.Action;
using UnityEngine;

namespace NueGames.Data.Collection
{
    public class DropdownTest : MonoBehaviour
    {
        
        [ShowInInspector]
        [FilePath]
        public static string AssetPath;
        
        [Searchable]
        [ValueDropdown("GetAssets", IsUniqueList = true)]
        [InlineEditor]
        public List<CardData> UniqueGameobjectList;
        
        [ShowInInspector]
        [TypeFilter("GetFilteredTypeList")]
        public List<GameActionBase> Array ;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(GameActionBase).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
                .Where(x => typeof(GameActionBase).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass

            // // Adds various C1<T> type variants.
            // q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(GameObject)));
            // q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(AnimationCurve)));
            // q = q.AppendWith(typeof(C1<>).MakeGenericType(typeof(List<float>)));

            return q;
        }
        
    
#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            var root = AssetPath;

            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith(root))
                .Select(x => x.Substring(root.Length))
                .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(root + x)));
        }
#endif
    }   
}