using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Settings;
using NueGames.Managers;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public GameplayData GameplayData ;
    GameManager GameManager = GameManager.Instance;
    
    void Awake()
    {
        SetGameplayDataAndStartGame();
    }

    private void SetGameplayDataAndStartGame()
    {
        GameManager.SetGameplayData(GameplayData);
        GameManager.StartRougeLikeGame();
    }

    
}
