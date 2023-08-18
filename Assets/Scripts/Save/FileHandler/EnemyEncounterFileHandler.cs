using System.Collections.Generic;
using NueGames.Data.Collection;
using NueGames.Data.Encounter;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataPersistence
{
    public class EnemyEncounterFileHandler : SerializedMonoBehaviour
    {
        [InlineEditor()]
        public ScriptableObjectReferenceCache dataReference;
        
        [Button]
        public List<string> DataToGuid(List<EnemyEncounter> cardDataList)
        {
            var guids = new List<string>();
            foreach (var cardData in cardDataList)
            {
                if (dataReference.CanReference(cardData, out string guid))
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
        public List<EnemyEncounter> GuidToData(List<string> guids)
        {
            var cardDatas = new List<EnemyEncounter>();
            foreach (var guid in guids)
            {
                if (dataReference.TryResolveReference(guid, out object outputCard))
                {
                    EnemyEncounter cardData = (EnemyEncounter)outputCard;
                    cardDatas.Add(cardData);
                }
            }

            return cardDatas;
        }
    }
}