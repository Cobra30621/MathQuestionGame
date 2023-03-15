using System.Collections;
using System.Collections.Generic;
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
        CreateMathQuestioningAction();
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
