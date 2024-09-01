using System;
using Card;
using Card.Data;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Utils
{
    public class InventoryHelper : MonoBehaviour
    {
        [SerializeField] private InventoryTypes inventoryType;
        
        private UIManager UIManager => UIManager.Instance;
        
        public void OpenInventory()
        {
            switch (inventoryType)
            {
                case InventoryTypes.CurrentDeck:
                    UIManager.OpenInventory(CardManager.Instance.CurrentCardsList,"Current Cards");
                    break;
                case InventoryTypes.DrawPile:
                    UIManager.OpenInventory(CollectionManager.Instance.DrawPile,"Draw Pile");
                    break;
                case InventoryTypes.DiscardPile:
                    UIManager.OpenInventory(CollectionManager.Instance.DiscardPile,"Discard Pile");
                    break;
                case InventoryTypes.ExhaustPile:
                    UIManager.OpenInventory(CollectionManager.Instance.ExhaustPile,"Exhaust Pile");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}