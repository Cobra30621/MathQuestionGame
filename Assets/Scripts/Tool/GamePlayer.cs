using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Settings;
using NueGames.Managers;
using Question;
using Sirenix.OdinInspector;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [InlineEditor()]
    public GameplayData GameplayData ;

    [SerializeField] private QuestionSettingUI questionSettingUI;

    public void SetGameplayDataAndStartGame()
    {
        GameManager.Instance.SetGameplayData(GameplayData);
        questionSettingUI.SetQuestionSetting();
        GameManager.Instance.StartRougeLikeGame();
    }

    
}
