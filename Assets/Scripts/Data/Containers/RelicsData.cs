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
    public class RelicsData : ScriptableObject
    {
        [SerializeField] private RelicIconsBase relicBasePrefab;
        [SerializeField] private List<RelicData> relicList;

        /// <summary>
        /// 能力 Icon 的遊戲物件(Prefab)
        /// </summary>
        public RelicIconsBase RelicBasePrefab => relicBasePrefab;
        /// <summary>
        /// 能力清單
        /// </summary>
        public List<RelicData> RelicList => relicList;

        public RelicData GetRelicData(RelicName relicName)
        {
            return RelicList.FirstOrDefault(x => x.RelicName == relicName);
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
        
        [SerializeField] private RelicName relicName;
        
        [SerializeField] private Sprite iconSprite;
        
        [LabelText("正在開發中的卡片")] 
        [SerializeField] private bool isDeveloping;
        
        /// <summary>
        /// 編號
        /// </summary>
        public RelicName RelicName => relicName;
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