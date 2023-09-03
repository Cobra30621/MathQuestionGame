using System.Collections;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Card
{
    public class BattleCard : CardBase
    {
        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;
      
        public override void SetCard(CardData cardData,bool isPlayable)
        {
            base.SetCard(cardData,isPlayable);
           
            if (canvas)
                canvas.worldCamera = CollectionManager.HandController.cam;
        }
    }
}