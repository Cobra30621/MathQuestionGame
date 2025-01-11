using Combat.Card;
using Managers;
using Map_System.Scripts.MapData;
using Relic.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Tool
{
    public class Tester : MonoBehaviour
    {
        public bool playOnStart;
        public UnityEvent testEvent;

        public MapConfig MapConfig;

        void Start()
        {
            if (playOnStart)
            {
                PlayTest();
            }
        }
    
        [ContextMenu("Play Test")]
        public void PlayTest()
        {
            testEvent.Invoke();
        
        
            // CollectionManager.Instance.ChangeHandCardManaCost(SpecialKeywords.MathMana, 0, false);
            // CardChoice();
            // GainRelic();
        }

        public ChoiceParameter ChoiceParameter;
    
        private void CardChoice()
        {
            CollectionManager.Instance.ShowChoiceCardPanel(ChoiceParameter);
        }

        public RelicName relicName;
        private void GainRelic()
        {
            GameManager.Instance.RelicManager.GainRelic(relicName);
        }


        void CreateMathQuestioningAction()
        {
            // EnterMathQuestioningAction enterMathQuestioningAction = new EnterMathQuestioningAction();
            // MathQuestioningActionParameters parameters = new MathQuestioningActionParameters();
            // parameters.SetQuestionCountValue(true, 3);
            // enterMathQuestioningAction.SetValue(parameters);
            // enterMathQuestioningAction.AddAction();
        }
    }
}
