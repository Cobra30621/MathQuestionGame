using Card;
using Economy;
using Log;
using Managers;
using NueGames.Data.Settings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Save
{
    public class FirstEnterGameHandler : MonoBehaviour
    {
        [Header("Settings")] [InlineEditor()] [Required] [SerializeField]
        private GameplayData gameplayData;


        private void Awake()
        {
            CheckFirstEnterGame();
        }


        [Button]
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
            EventLogger.Instance.LogEvent(LogEventType.Main, "創建初次進遊戲的資料","");
            CoinManager.Instance.SetMoney(gameplayData.InitMoney);
            CoinManager.Instance.SetStone(gameplayData.InitStone);
            CardManager.Instance.cardLevelHandler.InitDictionary();
            GameManager.Instance.RelicManager.relicLevelHandler.InitDictionary();

            SaveManager.Instance.SavePermanentGame();
            SaveManager.Instance.SetHaveEnteredGame();
        }
    }
}