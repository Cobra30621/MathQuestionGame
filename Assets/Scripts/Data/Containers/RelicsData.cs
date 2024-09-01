using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Relic;
using NueGames.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace  NueGames.Data.Containers
{
    /// <summary>
    /// 所有遺物的資料
    /// </summary>
    [CreateAssetMenu(fileName = "Relics", menuName = "NueDeck/Containers/Relics", order = 2)]
    public class RelicsData : SerializedScriptableObject
    {
        [SerializeField] private RelicIconsBase relicBasePrefab;

        [SerializeField] private Dictionary<RelicName, RelicData> _relicDict;
        

        public RelicIconsBase RelicBasePrefab => relicBasePrefab;
   
        public Dictionary<RelicName, RelicData> RelicDict => _relicDict;

        public RelicData GetRelicData(RelicName relicName)
        {
            if (_relicDict.TryGetValue(relicName, out var data))
            {
                return data;
            }
            else
            {
                Debug.LogError($"RelicData not found: {relicName}");
                return null;
            }
        }
    }


    [Serializable]
    public class RelicData
    {
        [LabelText("名稱")]
        public string Title;
        
        [LabelText("不同等級遺物描述")]
        [SerializeField]
        [ValidateInput("@descriptions.Count>1", "描述數量必須大於1(包含升級後)")]
        private List<string> descriptions;
        
        
        [SerializeField] private Sprite iconSprite;
        
        [LabelText("正在開發中的卡片")] 
        [SerializeField] private bool isDeveloping;
        
        /// <summary>
        /// Icon
        /// </summary>
        public Sprite IconSprite => iconSprite;

        public bool IsDeveloping => isDeveloping;
        
        public string GetDescription(int level)
        {
            return descriptions[level];
        }
    }
}