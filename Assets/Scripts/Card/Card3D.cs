using System.Collections;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Card
{
    public class Card3D : CardBase
    {
        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;
      
        public override void SetCard(CardData targetProfile,bool isPlayable)
        {
            base.SetCard(targetProfile,isPlayable);
           
            if (canvas)
                canvas.worldCamera = CollectionManager.HandController.cam;
        }
        
        public override void SetInactiveMaterialState(bool isInactive)
        {
            base.SetInactiveMaterialState(isInactive);
        }
    }
}