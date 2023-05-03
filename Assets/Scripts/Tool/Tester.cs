using System.Collections;
using System.Collections.Generic;
using Managers;
using Map;
using NueGames.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Relic;
using UnityEngine;
using UnityEngine.Events;

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
        
        MapConfig.encounterStage.Test();
        
        
        // CollectionManager.Instance.ChangeHandCardManaCost(SpecialKeywords.MathMana, 0, false);
        // CardChoice();
        // GainRelic();
    }

    public ChoiceParameter ChoiceParameter;
    
    private void CardChoice()
    {
        CollectionManager.Instance.ShowChoiceCardPanel(ChoiceParameter);
    }

    public RelicType RelicType;
    private void GainRelic()
    {
        RelicManager.Instance.GainRelic(RelicType);
        RelicManager.Instance.PrintCurrentRelicList();
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
