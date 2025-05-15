using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Effect;
using NueGames.Enums;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
{
    /// <summary>
    /// 卡牌等級資料的 ScriptableObject
    /// 從指定的 Google 表單載入並解析卡牌等級資訊
    /// </summary>
    [CreateAssetMenu(fileName = "CardLevelData", menuName = "CardLevelData", order = 0)]
    public class CardLevelData : ScriptableObject
    {
        /// <summary>
        /// 資料是否正在載入中
        /// </summary>
        public bool IsLoading { get; private set; }
        
        #region 私有變數

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/CardLevelData_dev";
        // Google 表單資料的網址

        [SerializeField]
        [TableList]
        private CardLevelInfo[] cardInfos;
        // 解析後的卡牌等級資訊陣列

        #endregion
        
        
        #region 公開方法

        /// <summary>
        /// 取得所有卡牌等級資訊
        /// </summary>
        public List<CardLevelInfo> GetAllCardInfo()
        {
            return cardInfos.ToList();
        }

        /// <summary>
        /// 根據指定的卡牌 GroupID，取得該卡牌所有等級的資訊
        /// </summary>
        /// <param name="cardId">卡牌的 GroupID</param>
        public List<CardLevelInfo> GetLevelInfo(string cardId)
        {
            return GetAllCardInfo()
                .Where(x => x.GroupID == cardId)
                .ToList();
        }

        /// <summary>
        /// 取得所有不同的卡牌 GroupID 列表
        /// </summary>
        public List<string> GetGroupIds()
        {
            var groupIds = cardInfos
                .Select(card => card.GroupID)
                .Distinct()
                .ToList();
            return groupIds;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 在 Inspector 中顯示的按鈕，觸發從 Google 表單解析資料
        /// </summary>
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<CardLevelInfo>(url, infos =>
            {
                // 剔除空白列（ID 欄位為空白）
                cardInfos = infos
                    .Where(info => !info.ID.IsNullOrWhitespace())
                    .ToArray();

                Debug.Log($"卡牌等級資訊總筆數: {infos.Length}");

                // 將 SkillID 的字串轉成清單
                foreach (var cardLevelInfo in cardInfos)
                {
                    cardLevelInfo.skillIDs = Helper.ConvertStringToStringList(cardLevelInfo.SkillID);
                }

                IsLoading = false;
            });
        }

        #endregion
    }
    
    /// <summary>
    /// 單筆卡牌等級資訊的資料結構
    /// </summary>
    [Serializable]
    public class CardLevelInfo
    {
        /// <summary>唯一識別此筆資料的 ID</summary>
        public string ID;

        /// <summary>卡牌分組 ID，同一張卡不同等級共用此 GroupID</summary>
        public string GroupID;

        /// <summary>從表單讀取的原始技能 ID 字串（以逗號分隔）</summary>
        public string SkillID;
        
        /// <summary>解析後的技能 ID 清單</summary>
        public List<string> skillIDs;
        
        /// <summary>解析後對應的 SkillInfo 物件清單</summary>
        public List<SkillInfo> SkillInfos;

        /// <summary>此等級卡牌的法力消耗</summary>
        public int ManaCost;

        /// <summary>升級到此等級所需的金幣數量</summary>
        public int UpgradeCost;

        /// <summary>升級到此等級所需的寶石數量</summary>
        public int UpgradeCostStone;

        /// <summary>卡牌所屬的職業類型</summary>
        public AllyClassType Class;

        /// <summary>是否為此卡的最高等級</summary>
        public bool MaxLevel;

        /// <summary>卡牌等級的標題</summary>
        public string TitleLang;

        /// <summary>卡牌等級的描述</summary>
        public string DesLang;

        /// <summary>這張卡片可以選擇的目標對象</summary>
        public ActionTargetType TargetChoose;

        /// <summary>是否在使用後移除</summary>
        public bool ExhaustAfterPlay;

        
    }
}
