using System;
using Card;
using Money;
using NueGames.Data.Settings;
using Relic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Managers
{
    public class FirstEnterGameHandler : MonoBehaviour
    {
        [Header("Settings")]
        [InlineEditor()]
        [Required]
        [SerializeField] private GameplayData gameplayData;


        public void CheckFirstEnterGame()
        {
            if (SaveManager.Instance.IsFirstEnterGame())
            {
                CreateInitData();
            }
            else
            {
                SaveManager.Instance.LoadPermanentGame();
            }
        }

        private void CreateInitData()
        {
            Debug.Log("創建初次進遊戲的資料");
            CoinManager.Instance.SetMoney(gameplayData.InitMoney);
            CoinManager.Instance.SetStone(gameplayData.InitStone);
            CardManager.Instance.CardLevelHandler.InitDictionary();
            GameManager.Instance.RelicManager.relicLevelHandler.InitRelicSaveInfo();
                
            SaveManager.Instance.SavePermanentGame();
            SaveManager.Instance.SetHaveEnterGame();
        }
    }
}