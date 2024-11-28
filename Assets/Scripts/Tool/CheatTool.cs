using System.Collections.Generic;
using Action.Parameters;
using Combat;
using Money;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tool
{
    public class CheatTool : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject cheatPanel;    // 作弊面板
        [SerializeField] private Button openCheatButton;   // 開啟作弊面板的按鈕
        [SerializeField] private Button closeCheatButton;  // 關閉作弊面板的按鈕
        [SerializeField] private TMP_InputField moneyInput;
        [SerializeField] private TMP_InputField stoneInput;
        [SerializeField] private Button addMoneyButton;
        [SerializeField] private Button addStoneButton;
        [SerializeField] private Button killAllButton;

        private void Start()
        {
            // 初始化時隱藏面板
            if (cheatPanel != null)
                cheatPanel.SetActive(false);
            
            // 設置開啟和關閉按鈕的監聽器
            if (openCheatButton != null)
                openCheatButton.onClick.AddListener(OpenCheatPanel);
            
            if (closeCheatButton != null)
                closeCheatButton.onClick.AddListener(CloseCheatPanel);
            
            // 設置按鈕監聽器
            addMoneyButton.onClick.AddListener(OnAddMoneyClick);
            addStoneButton.onClick.AddListener(OnAddStoneClick);
            killAllButton.onClick.AddListener(KillAllEnemy);
        }

        private void OnAddMoneyClick()
        {
            if (int.TryParse(moneyInput.text, out int amount))
            {
                AddMoney(amount);
            }
        }

        private void OnAddStoneClick()
        {
            if (int.TryParse(stoneInput.text, out int amount))
            {
                AddStone(amount);
            }
        }

        public void AddMoney(int money)
        {
            CoinManager.Instance.AddCoin(money, CoinType.Money);
        }

        public void AddStone(int stone)
        {
            CoinManager.Instance.AddCoin(stone, CoinType.Stone);
        }

        public void KillAllEnemy()
        {
            List<CharacterBase> targets = new List<CharacterBase>();
            var allEnemy = CombatManager.Instance.Enemies;
            targets.AddRange(allEnemy);
            
            var damageInfo = new DamageInfo(999, new ActionSource(), fixDamage: true, canPierceArmor:true);

            GameActionExecutor.AddAction(new DamageAction(damageInfo, targets));
        }

        public void OpenCheatPanel()
        {
            if (cheatPanel != null)
                cheatPanel.SetActive(true);
        }

        public void CloseCheatPanel()
        {
            if (cheatPanel != null)
                cheatPanel.SetActive(false);
        }
    }
}