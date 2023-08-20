using System.Collections;
using System.Collections.Generic;
using CampFire;
using NueGames.CharacterAbility;
using NueGames.Data.Collection;
using NueGames.UI;
using NueGames.UI.Reward;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-4)]
    public class UIManager : Singleton<UIManager>
    {
        [Header("Canvases")]
        [SerializeField] private CombatCanvas combatCanvas;
        [SerializeField] private InformationCanvas informationCanvas;
        [SerializeField] private RewardCanvas rewardCanvas;
        [SerializeField] private InventoryCanvas inventoryCanvas;
        [SerializeField] private RelicCanvas relicCanvas;
        [SerializeField] private CampFireCanvas campFireCanvas;
        [SerializeField] private CharacterSkillUI characterSkillUI;
        

        [Header("Fader")]
        [SerializeField] private CanvasGroup fader;
        [SerializeField] private float fadeSpeed = 1f;


        #region Cache
        public CombatCanvas CombatCanvas => combatCanvas;
        public InformationCanvas InformationCanvas => informationCanvas;
        public RewardCanvas RewardCanvas => rewardCanvas;
        public InventoryCanvas InventoryCanvas => inventoryCanvas;
        public RelicCanvas RelicCanvas => relicCanvas;
        public CampFireCanvas CampFireCanvas => campFireCanvas;
        public CharacterSkillUI CharacterSkillUI => characterSkillUI;
        
        #endregion

        #region Public Methods

        public void OpenInventory(List<CardData> cardList,string title)
        {
           SetCanvas(InventoryCanvas,true,true);
           InventoryCanvas.ChangeTitle(title);
           InventoryCanvas.SetCards(cardList);
        }
        
        public void SetCanvas(CanvasBase targetCanvas,bool open,bool reset = false)
        {
            if (reset)
                targetCanvas.ResetCanvas();
            
            if (open)
                targetCanvas.OpenCanvas();
            else
                targetCanvas.CloseCanvas();
        }
        public void ChangeScene(int index)
        {
            StartCoroutine(ChangeSceneRoutine(index));
        }
        #endregion
        
        #region Routines
        private IEnumerator ChangeSceneRoutine(int index)
        {
            SceneManager.LoadScene(index);
            yield return StartCoroutine(Fade(false));
        }
        
        public IEnumerator Fade(bool isIn)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = isIn ? 0f : 1f;

            while (true)
            {
                timer += Time.deltaTime* (isIn ? fadeSpeed : -fadeSpeed);
                
                fader.alpha = timer;
                
                if (timer>=1f)  break;
              
                yield return waitFrame;
            }
        }

        #endregion
       
    }
}
