using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Sirenix.OdinInspector;
using Tool;
using UnityEditor;
using UnityEngine;

namespace Card
{
    public class CardEditorTool : MonoBehaviour
    {
        [InlineEditor]
        public DeckData SaveDeck;
        [InlineEditor]
        public CardLevelData cardLevelData;
        [InlineEditor]
        public SkillData skillData;

        [Button("讀取卡牌資料")]
        public void LoadCardData()
        {
            
            foreach (var card in SaveDeck.CardList)
            {
                var levelInfos = GetLevelInfo( card.CardId);
                
                foreach (var levelInfo in levelInfos)
                {
                    var skillId = Helper.ConvertStringToIntList(levelInfo.SkillID);
                    var effectInfos = GetSkillInfo(skillId);
                    levelInfo.SetEffect(effectInfos);
                }
                    
                card.SetCardLevels(levelInfos);
                EditorUtility.SetDirty(card);
            }
            
            EditorUtility.SetDirty(SaveDeck);
            EditorUtility.SetDirty(cardLevelData);
            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
        }

        private List<CardLevelInfo> GetLevelInfo(string cardId)
        {
            return cardLevelData.GetAllCardInfo().
                Where(x => x.GroupID == cardId).ToList();
        }

        private List<SkillInfo> GetSkillInfo(List<int> skillId)
        {
            return skillData.GetSkillInfos().Where(x =>  skillId.Contains(x.SkillID)).ToList();
        }
        
        
    }
}