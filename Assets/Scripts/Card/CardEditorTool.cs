using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Sirenix.OdinInspector;
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
                    var effectId = ConvertStringToList(levelInfo.EffectID);
                    var effectInfos = GetEffectInfo(effectId);
                    levelInfo.SetEffect(effectInfos);
                }
                    
                card.SetCardLevels(levelInfos);
            }
            
            AssetDatabase.SaveAssets();
        }

        private List<CardLevelInfo> GetLevelInfo(string cardId)
        {
            return cardLevelData.GetAllCardInfo().
                Where(x => x.GroupID == cardId).ToList();
        }

        private List<SkillInfo> GetEffectInfo(List<int> effectId)
        {
            return skillData.GetAllCardInfo().Where(x =>  effectId.Contains(x.SkillID)).ToList();
        }
        
        static List<int> ConvertStringToList(string input)
        {
            List<int> numbers = new List<int>();

            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int number))
                {
                    numbers.Add(number);
                }
                else
                {
                    Debug.Log($"Invalid input: {part}");
                }
            }

            return numbers;
        }
    }
}