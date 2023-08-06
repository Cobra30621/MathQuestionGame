using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Settings;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [InlineEditor()]
    public GameplayData GameplayData ;

    public void SetGameplayDataAndStartGame()
    {
        GameManager.Instance.SetGameplayData(GameplayData);
        GameManager.Instance.StartRougeLikeGame();
    }

    
}
