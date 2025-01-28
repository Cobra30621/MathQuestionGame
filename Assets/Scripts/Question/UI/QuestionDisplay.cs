using System.Collections;
using Question.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question.UI
{
    /// <summary>
    /// 負責顯示答題介面的資訊
    /// </summary>
    public class QuestionDisplay : MonoBehaviour
    {
        /// <summary>
        /// 題目的圖片
        /// </summary>
        [Required]
        [SerializeField] private Image qeustionImage;
        /// <summary>
        /// 選項的圖片
        /// </summary>
        [Required]
        [SerializeField] private Image optionImage;

        /// <summary>
        /// 所需答對的題數
        /// </summary>
        [Required]
        [SerializeField] private TextMeshProUGUI needAnswerCount;

        /// <summary>
        /// 顯示的資訊
        /// </summary>
        [Required]
        [SerializeField] private TextMeshProUGUI infoText;
        
        
        
        /// <summary>
        /// 動畫
        /// </summary>
        [Required]
        [SerializeField] private Animator animator;
        
        private static readonly int InQuestioningMode = Animator.StringToHash("In Questioning Mode");
        private static readonly int ShowQuestion = Animator.StringToHash("Show Question");
        private static readonly int OnAnswerQuestion = Animator.StringToHash("On Answer");

        [ShowInInspector]
        public bool WaitingForAnimation { private set; get; }
        
        /// <summary>
        /// 進入答題介面
        /// </summary>
        public IEnumerator EnterQuestionMode()
        {
            WaitingForAnimation = true; // 設定目前正在播動畫
            animator.SetBool(InQuestioningMode, true);
            
            yield return new WaitUntil(()=> !WaitingForAnimation); 
        }
        
        
        
        public IEnumerator SetNextQuestion(Data.Question question)
        {
            qeustionImage.sprite = question.QuestionSprite;
            optionImage.sprite = question.OptionSprite;

            WaitingForAnimation = true; // 設定目前正在播動畫
            animator.SetTrigger(ShowQuestion);
            
            yield return new WaitUntil(()=> !WaitingForAnimation); 
        }

        /// <summary>
        /// 設定目前的答題紀錄
        /// </summary>
        /// <param name="questionSession"></param>
        public void UpdateAnswerRecord(QuestionSession questionSession)
        {
            int questionCount = questionSession.QuestionCount;
            int leastQuestionCount = questionCount - questionSession.HasAnswerCount;
            needAnswerCount.text = $"{leastQuestionCount}";
        }

        public void HandleAnswer()
        {
            WaitingForAnimation = true; // 設定目前正在播動畫
            animator.SetTrigger(OnAnswerQuestion);
        }
        
        public IEnumerator ExitQuestionMode(string info)
        {
            infoText.text = info;
            
            WaitingForAnimation = true; // 設定目前正在播動畫
            animator.SetBool(InQuestioningMode, false);
            
            yield return new WaitUntil(()=> !WaitingForAnimation); 
        }
        
        
        /// <summary>
        /// 設置動畫播放完畢(給 animator 使用)
        /// </summary>
        public void OnAnimationFinish()
        {
            WaitingForAnimation = false;
        }
    }
}