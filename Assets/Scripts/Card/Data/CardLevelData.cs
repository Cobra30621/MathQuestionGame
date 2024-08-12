using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Enums;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
{
    [CreateAssetMenu(fileName = "CardLevelData",menuName = "CardLevelData",order = 0)]
    public class CardLevelData :  ScriptableObject
    {
        public bool IsLoading { get; private set; }
        
        #region Private Variables

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/CardLevelData_dev";

        [SerializeField]
        [TableList]
        private CardLevelInfo[] cardInfos;

        #endregion
        
        
        #region Public Methods

        public List<CardLevelInfo> GetAllCardInfo()
        {
            return cardInfos.ToList();
        }

        public List<CardLevelInfo> GetLevelInfo(string cardId)
        {
            return GetAllCardInfo().
                Where(x => x.GroupID == cardId).ToList();
        }

        public List<string> GetGroupIds()
        {
            var groupIds = cardInfos.
                Select(card => card.GroupID).
                Distinct().
                ToList();
            return groupIds;
        }

        #endregion

        #region Private Methods

        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<CardLevelInfo>(url , infos =>
            {
                cardInfos = infos;
                Debug.Log($"CardLevelInfo Count: {infos.Length}");
                
                foreach (var cardLevelInfo in cardInfos)
                {
                    cardLevelInfo.skillIDs = Helper.ConvertStringToStringList(cardLevelInfo.SkillID);
                }
                
                IsLoading = false;
            });
        }

        #endregion
        
        
        
    }
    
    [Serializable]
    public class CardLevelInfo
    {
        public int ID;
        public string GroupID;
        public string SkillID;
        public int ManaCost;
        public int UpgradeCost;
        public AllyClassType Class;
        public bool MaxLevel;
        public string TitleLang;
        public string DesLang;
        public ActionTargetType TargetChoose;
        
        public List<string> skillIDs;
        
        public List<SkillInfo> EffectInfos;
    }

}