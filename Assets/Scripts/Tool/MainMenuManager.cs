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
    [LabelText("關卡資料")]
    public GameplayData GameplayData ;

    [SerializeField] private QuestionSettingUI questionSettingUI;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private OnlineQuestionDownloader onlineQuestionDownloader;
    public void NewGame()
    {
        GameManager.Instance.SetGameplayData(GameplayData);
        questionSettingUI.SetQuestionSetting();
        GameManager.Instance.NewGame();
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        onlineQuestionDownloader.GetQuestion();
        yield return new WaitForSeconds(5);
        sceneChanger.OpenMapScene();
    }
    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
        sceneChanger.OpenMapScene();
    }

    public void ExitGame()
    {
        sceneChanger.ExitApp();
    }
    
}
