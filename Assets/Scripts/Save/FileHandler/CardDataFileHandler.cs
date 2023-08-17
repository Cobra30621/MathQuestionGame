using System;
using System.Collections.Generic;
using Data;
using NueGames.Data.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataPersistence
{
    public class CardDataFileHandler : SerializedMonoBehaviour
    {
        [InlineEditor()]
        public ScriptableObjectReferenceCache CardDataReference;
        
        // [ReadOnly]
        // [SerializeField]
        // private List<string> guids;
        //
        // [ReadOnly]
        // [SerializeField]
        // private List<CardData> cardDatas;
        
        // [Button]
        // public void TestSave()
        // {
        //     DataToGuid(cardDatas);
        // }

        [Button]
        public List<string> DataToGuid(List<CardData> cardDataList)
        {
            var guids = new List<string>();
            foreach (var cardData in cardDataList)
            {
                if (CardDataReference.CanReference(cardData, out string guid))
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
                if (CardDataReference.TryResolveReference(guid, out object outputCard))
                {
                    CardData cardData = (CardData)outputCard;
                    cardDatas.Add(cardData);
                }
            }

            return cardDatas;
        }
    }
}