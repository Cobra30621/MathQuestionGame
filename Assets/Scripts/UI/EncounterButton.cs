using System;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.UI
{
    public class EncounterButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Button button;
        [SerializeField] private Image completedImage;
        [SerializeField] private bool isFinal;
        
        protected GameManager GameManager => GameManager.Instance;
        
        private void Awake()
        {
            completedImage.gameObject.SetActive(false);
        }
        
        public void SetStatus(EncounterButtonStatus targetStatus)
        {
            switch (targetStatus)
            {
                case EncounterButtonStatus.Active:
                    button.interactable = true;
                    if (isFinal) GameManager.PlayerData.IsFinalEncounter = true;
                    
                    break;
                case EncounterButtonStatus.Passive:
                    button.interactable = false;
                    break;
                case EncounterButtonStatus.Completed:
                    button.interactable = false;
                    completedImage.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStatus), targetStatus, null);
            }
        }
    }
}