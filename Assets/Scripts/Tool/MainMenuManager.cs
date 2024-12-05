using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Data.Characters;
using NueGames.Data.Settings;
using NueGames.Managers;
using NueGames.Utils;
using Question;
using Question.QuestionLoader;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private SceneChanger sceneChanger;
    
    
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
