using System.Collections;
using Card.Data;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Card
{
    public class BattleBattleCardTemp : BattleCard
    {
        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;
      
        public override void Init(CardData cardData)
        {
            base.Init(cardData);
           
            if (canvas)
                canvas.worldCamera = CollectionManager.HandController.cam;
        }
    }
}