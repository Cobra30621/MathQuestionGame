using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.UI;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NueGames.Combat;
using NueGames.Power;
using NueTooltip.Core;

namespace NueGames.Characters
{
    [RequireComponent(typeof(Canvas))]
    public class CharacterCanvas : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Transform statusIconRoot;
        [SerializeField] protected Transform highlightRoot;
        
        [SerializeField] protected PowersData powersData;

        [SerializeField] protected TextMeshProUGUI currentHealthText;
        [SerializeField] protected Image currentHealthBar;
        
        #region Cache
        
        protected Dictionary<PowerName, PowerIconsBase> StatusDict = new Dictionary<PowerName, PowerIconsBase>();

        protected Canvas TargetCanvas;


        #endregion
        
        #region Setup

        public void InitCanvas()
        {
            highlightRoot.gameObject.SetActive(false);

            TargetCanvas = GetComponent<Canvas>();

            if (TargetCanvas)
                TargetCanvas.worldCamera = Camera.main;
        }

        #endregion
        
        #region Public Methods
        public void ApplyStatus(PowerName targetPower, int value)
        {
            // 格檔不需要在上面
            if (targetPower == PowerName.Block)
            {
                return;
            }
            
            if (!StatusDict.ContainsKey(targetPower))
            {
                var targetData = powersData.GetPowerData(targetPower);
                var clone = Instantiate(powersData.PowerBasePrefab, statusIconRoot);
                clone.SetStatus(targetData);
                StatusDict.Add(targetPower, clone);
            }
            
            StatusDict[targetPower].SetStatusValue(value);
        }

        public void ClearStatus(PowerName targetPower)
        {
            if (StatusDict.ContainsKey(targetPower))
            {
                if(StatusDict[targetPower] != null)
                    Destroy(StatusDict[targetPower].gameObject);
                
                StatusDict.Remove(targetPower);
            }
        }
        
        public void UpdateStatusText(PowerName targetPower, int value)
        {
            if (!StatusDict.ContainsKey(targetPower)) return;
          
            StatusDict[targetPower].StatusValueText.text = $"{value}";
        }

        public void UpdateHealthInfo(int currentHealth, int maxHealth)
        {
            currentHealthText.text = $"{currentHealth}/{maxHealth}";
            currentHealthBar.fillAmount = (float)currentHealth / maxHealth;
        }  
        public void SetHighlight(bool open) => highlightRoot.gameObject.SetActive(open);
       
        #endregion

        
        
       
    }
}