using System.Collections;
using Feedback;
using Question.Answer_Button;
using Question.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.UI
{
    /// <summary>
    /// 負責控制答題相關的 UI 
    /// </summary>
    public class QuestionUIController : MonoBehaviour
    {
        /// <summary>
        /// 答題按鈕
        /// </summary>
        [Required]
        [SerializeField] private AnswerButtonBase[] answerButtons;

        /// <summary>
        /// 題目資訊顯示
        /// </summary>
        [Required]
        [SerializeField] private QuestionDisplay display;
        
        /// <summary>
        /// 主介面
        /// </summary>
        [Required]
        [SerializeField] private GameObject mainPanel;
        
        /// <summary>
        /// 答對的反饋
        /// </summary>
        [Required]
        [SerializeField] private IFeedback onAnswerCorrectFeedback;
        
        /// <summary>
        /// 答錯的反饋
        /// </summary>
        [Required]
        [SerializeField] private IFeedback onAnswerWrongFeedback;

        
        /// <summary>
        /// 進入答題介面
        /// </summary>
        public IEnumerator EnterQuestionMode()
        {
            mainPanel.SetActive(true);
            
            // 等待進入答題介面的動畫播放完畢
            yield return display.EnterQuestionMode();
        }

        /// <summary>
        /// 離開答題介面
        /// </summary>
        /// <param name="info"></param>
        public IEnumerator ExitQuestionMode(string info)
        {
            // 等待離開答題介面的動畫播放完畢
            yield return display.ExitQuestionMode(info); 
            yield return new WaitForSeconds(0.5f);
            
            mainPanel.SetActive(false);
        }

        /// <summary>
        /// 顯示下一題
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public IEnumerator ShowNextQuestion(Data.Question question)
        {
            yield return display.SetNextQuestion(question);
        }
        

        
        /// <summary>
        /// 播放答題後的反饋
        /// </summary>
        /// <param name="correct"></param>
        /// <param name="option"></param>
        public void HandleAnswerFeedback(bool correct, int option)
        {
            display.HandleAnswer();
            
            // answerButtons[option]?.PlayOnAnswerAnimation(correct);
            if (correct)
            {
                onAnswerCorrectFeedback.Play();
            }
            else
            {
                onAnswerWrongFeedback.Play();
            }
        }

        /// <summary>
        /// 更新答題紀錄
        /// </summary>
        /// <param name="session"></param>
        public void UpdateAnswerRecord(QuestionSession session)
        {
            display.UpdateAnswerRecord(session);
        }

        /// <summary>
        /// 設定可以答題
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableAnswer(bool enable)
        {
            foreach (var answerButton in answerButtons)
            {
                answerButton.EnableAnswer(enable);
            }
        }
    }
}

