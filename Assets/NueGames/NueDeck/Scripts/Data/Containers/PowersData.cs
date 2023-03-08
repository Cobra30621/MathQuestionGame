using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.UI;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Containers
{
    [CreateAssetMenu(fileName = "Powers", menuName = "NueDeck/Containers/Powers", order = 2)]
    public class PowersData : ScriptableObject
    {
        [SerializeField] private PowerIconsBase powerBasePrefab;
        [SerializeField] private List<PowerData> powerList;

        public PowerIconsBase PowerBasePrefab => powerBasePrefab;
        public List<PowerData> PowerList => powerList;

        public PowerData GetPowerData(PowerType powerType)
        {
            return PowerList.FirstOrDefault(x => x.PowerType == powerType);
        }
    }


    [Serializable]
    public class PowerData
    {
        [SerializeField] private string titleText;
        [SerializeField][TextArea] private string contentText;
        [SerializeField] private PowerType powerType;
        [SerializeField] private Sprite iconSprite;
        public PowerType PowerType => powerType;
        public Sprite IconSprite => iconSprite;
        
        
        
        
        public string GetHeader(string overrideKeywordHeader = "")
        {
            if(titleText != "")
                return titleText;
            return string.IsNullOrEmpty(overrideKeywordHeader) ? powerType.ToString() : overrideKeywordHeader;
        }

        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }
    }
}