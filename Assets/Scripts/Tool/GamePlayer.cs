using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Settings;
using NueGames.NueDeck.Scripts.Managers;
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
