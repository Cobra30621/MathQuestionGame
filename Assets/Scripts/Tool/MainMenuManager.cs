using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Data.Characters;
using NueGames.Data.Settings;
using NueGames.Managers;
using NueGames.Utils;
using Question;
using Question.QuestionLoader;
using Save;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Required]
    [SerializeField] private SceneChanger sceneChanger;

    [Required]
    [SerializeField] private GameObject continueButton;
    
    
    
    private void Awake()
    {
        // 有單局遊戲存檔資料，才會讓繼續按鈕出現
        var hasOngoingGame = SaveManager.Instance.HasOngoingGame();
        continueButton.SetActive(hasOngoingGame);
    }


    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
        StartCoroutine(sceneChanger.OpenMapScene());
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
