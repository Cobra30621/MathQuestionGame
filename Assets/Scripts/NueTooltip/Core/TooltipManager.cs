using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Data.Containers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using UnityEngine;

namespace NueTooltip.Core
{
    public class TooltipManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TooltipController tooltipController;
   
        [SerializeField] private TooltipText tooltipTextPrefab;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private SpecialKeywordData specialKeywordData;
        [SerializeField] private PowersData powersData;
        
        [Header("Settings")]
        [SerializeField] private AnimationCurve fadeCurve;
        [SerializeField] private float showDelayTime = 0.5f;
        [SerializeField] private bool canChangeCursor;

        public SpecialKeywordData SpecialKeywordData => specialKeywordData;
        public PowersData PowersData => powersData;
     
        private List<TooltipText> _tooltipTextList = new List<TooltipText>();
        private TooltipController TooltipController => tooltipController;

        private int _currentShownTooltipCount;

        public static TooltipManager Instance => GameManager.Instance.TooltipManager;
    

        private IEnumerator ShowRoutine(float delay = 0)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;
            
            
            canvasGroup.alpha = 0;

            yield return new WaitForSeconds(delay);
            
            while (true)
            {
                timer += Time.deltaTime;

                var invValue = Mathf.InverseLerp(0, showDelayTime, timer);
                canvasGroup.alpha = fadeCurve.Evaluate(invValue);
                
                if (timer>=showDelayTime)
                { 
                    canvasGroup.alpha = 1;
                    break;
                }
                yield return waitFrame;
            }
        }
        public void ShowTooltip(string contentText="",string headerText ="",Transform tooltipTargetTransform = null,
             Camera cam = null, float delayShow =0, bool focusFollow = true)
        {
            StartCoroutine(ShowRoutine(delayShow));
            _currentShownTooltipCount++;
            if (_tooltipTextList.Count<_currentShownTooltipCount)
            {
                var newTooltip = Instantiate(tooltipTextPrefab, TooltipController.transform);
                _tooltipTextList.Add(newTooltip);
            }
            
            _tooltipTextList[_currentShownTooltipCount-1].gameObject.SetActive(true);
            _tooltipTextList[_currentShownTooltipCount-1].SetText(contentText,headerText);
            
            TooltipController.SetFollowPos(tooltipTargetTransform, cam, focusFollow);
            
            
        }

        public void HideTooltip()
        {
            StopAllCoroutines();
            _currentShownTooltipCount = 0;
            canvasGroup.alpha = 0;
            foreach (var tooltipText in _tooltipTextList)
                tooltipText.gameObject.SetActive(false);

           
        }
        
    }
}