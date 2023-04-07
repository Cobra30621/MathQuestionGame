using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Relic;
using NueGames.UI;
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

        public RelicData GetRelicData(RelicType relicType)
        {
            return RelicList.FirstOrDefault(x => x.RelicType == relicType);
        }
    }
    
    
    [Serializable]
    public class RelicData
    {
        [SerializeField] private string titleText;
        
        [SerializeField][TextArea] private string contentText;
        
        [SerializeField] private RelicType relicType;
        
        [SerializeField] private Sprite iconSprite;
        
        /// <summary>
        /// 編號
        /// </summary>
        public RelicType RelicType => relicType;
        /// <summary>
        /// Icon
        /// </summary>
        public Sprite IconSprite => iconSprite;
        
        /// <summary>
        /// 名稱
        /// </summary>
        public string GetHeader(string overrideKeywordHeader = "")
        {
            if(titleText != "")
                return titleText;
            return string.IsNullOrEmpty(overrideKeywordHeader) ? relicType.ToString() : overrideKeywordHeader;
        }

        /// <summary>
        /// 能力說明
        /// </summary>
        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }

        public override string ToString()
        {
            return $"{nameof(titleText)}: {titleText}, {nameof(contentText)}: {contentText}, {nameof(relicType)}: {relicType}";
        }
    }
}