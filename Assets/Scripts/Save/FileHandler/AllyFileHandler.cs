using System.Collections.Generic;
using Card.Data;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataPersistence
{
    // TODO 要改成 GameObject 形式
    public class AllyFileHandler : SerializedMonoBehaviour
    {
        [InlineEditor()] public ScriptableObjectReferenceCache AllyDataReference;
        
        [Button]
        public List<string> DataToGuid(List<CardData> cardDataList)
        {
            var guids = new List<string>();
            foreach (var cardData in cardDataList)
            {
                if (AllyDataReference.CanReference(cardData, out string guid))
                {
                    guids.Add(guid);
                }
                else
                {
                    Debug.Log("無法轉換");
                }
            }

            return guids;
        }

        [Button]
        public List<CardData> GuidToData(List<string> guids)
        {
            var cardDatas = new List<CardData>();
            foreach (var guid in guids)
            {
                if (AllyDataReference.TryResolveReference(guid, out object outputCard))
                {
                    CardData cardData = (CardData)outputCard;
                    cardDatas.Add(cardData);
                }
            }

            return cardDatas;
        }
    }
}