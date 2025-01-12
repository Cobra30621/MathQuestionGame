using System.Collections.Generic;
using Characters;
using Combat;
using Economy;
using Effect;
using Effect.Damage;
using Effect.Parameters;
using Save;
using Sheets;
using Sirenix.OdinInspector;
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
        [SerializeField] private Button clearSaveDataButton;
        [SerializeField] private Button loadSheetButton;

        [Required]
        [SerializeField] private SheetDataLoader _sheetDataLoader;
        
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
            clearSaveDataButton.onClick.AddListener(ClearSaveData);
            loadSheetButton.onClick.AddListener(LoadSheet);
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
            
            var damageInfo = new DamageInfo(999, new EffectSource(), fixDamage: true, canPierceArmor:true);

            EffectExecutor.AddEffect(new DamageEffect(damageInfo, targets));
        }

        public void ClearSaveData()
        {
            SaveManager.Instance.ClearAllData();
        }

        public void OpenCheatPanel()
        {
            if (cheatPanel != null)
                cheatPanel.SetActive(!cheatPanel.activeSelf);
        }

        public void CloseCheatPanel()
        {
            if (cheatPanel != null)
                cheatPanel.SetActive(false);
        }
        
        public void LoadSheet()
        {
            Debug.Log("開始讀取表單");
            _sheetDataLoader.Load();
        }
    }
}