using System;
using Card;
using Card.Data;
using Managers;
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
                    UIManager.OpenInventory(CardManager.Instance.CurrentCardsList,"牌組");
                    break;
                case InventoryTypes.DrawPile:
                    UIManager.OpenInventory(CollectionManager.Instance.DrawPile,"抽牌堆");
                    break;
                case InventoryTypes.DiscardPile:
                    UIManager.OpenInventory(CollectionManager.Instance.DiscardPile,"棄牌堆");
                    break;
                case InventoryTypes.ExhaustPile:
                    UIManager.OpenInventory(CollectionManager.Instance.ExhaustPile,"被消耗的牌");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}