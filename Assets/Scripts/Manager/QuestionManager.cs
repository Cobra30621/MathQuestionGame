using NueGames.NueDeck.Scripts.Card;
using NueGames.Card.CardActions;
using UnityEngine;

namespace Managers
{
    public class QuestionManager : MonoBehaviour
    {
        private QuestionManager(){}
        public static QuestionManager Instance { get; private set; }

        public GameObject mainPanel;
        private CardActionParameters tempCardActionParameters;
        private MathActionBase tempMathAction;
        public int CorrectAnswer;
        
        
        #region Setup
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
            }
        }

        #endregion
        
        #region Public Methods

        public void EnterQuestion(MathActionBase mathAction)
        {
            tempMathAction = mathAction;
            mainPanel.SetActive(true);
        }

        public void InitQuestion()
        {
            CorrectAnswer = 0;
        }

        public void OnAnswer(int option)
        {
            if (option == CorrectAnswer)
            {
                tempMathAction.OnAnswer();
            }
            else
            {
                
            }
            mainPanel.SetActive(false);
        }
        
        
        
        #endregion
    }
}
