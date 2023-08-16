using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Settings;
using NueGames.Managers;
using NueGames.Utils;
using Question;
using Sirenix.OdinInspector;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [InlineEditor()]
    public GameplayData GameplayData ;

    [SerializeField] private QuestionSettingUI questionSettingUI;
    [SerializeField] private SceneChanger sceneChanger;
    
    public void NewGame()
    {
        GameManager.Instance.SetGameplayData(GameplayData);
        questionSettingUI.SetQuestionSetting();
        GameManager.Instance.NewGame();
        
        sceneChanger.OpenMapScene();
    }

    public void ContinueGame()
    {
        sceneChanger.OpenMapScene();
    }

    public void ExitGame()
    {
        sceneChanger.ExitApp();
    }
    
}
