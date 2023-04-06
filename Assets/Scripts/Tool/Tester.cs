using System.Collections;
using System.Collections.Generic;
using NueGames.Collection;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.Events;

public class Tester : MonoBehaviour
{
    public bool playOnStart;
    public UnityEvent testEvent;

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
        CardChoice();
    }

    public ChoiceParameter ChoiceParameter;
    
    private void CardChoice()
    {
        CollectionManager.Instance.ShowChoiceCardPanel(ChoiceParameter);
    }


    void CreateMathQuestioningAction()
    {
        // EnterMathQuestioningAction enterMathQuestioningAction = new EnterMathQuestioningAction();
        // MathQuestioningActionParameters parameters = new MathQuestioningActionParameters();
        // parameters.SetQuestionCountValue(true, 3);
        // enterMathQuestioningAction.SetValue(parameters);
        // enterMathQuestioningAction.AddToBottom();
    }
}
