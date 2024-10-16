using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Data.Characters;
using NueGames.Data.Settings;
using NueGames.Managers;
using NueGames.Utils;
using Question;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [InlineEditor()]
    [LabelText("玩家資料")]
    public AllyData allyData;
    
    [InlineEditor()]
    [LabelText("關卡資料")]
    public StageData stageData;

    [SerializeField] private QuestionSettingUI questionSettingUI;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private OnlineQuestionDownloader onlineQuestionDownloader;
    public void NewGame()
    {
        GameManager.Instance.SetAllyData(allyData);
        questionSettingUI.SetQuestionSetting();
        GameManager.Instance.NewGame();
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        // onlineQuestionDownloader.GetQuestion();
        // yield return new WaitForSeconds(5);
        yield return null;
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

    public void OpenShop()
    {
        UIManager.Instance.ShopCanvas.OpenCanvas();
    }
    
}
