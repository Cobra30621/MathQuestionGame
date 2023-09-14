using System;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NueGames.Utils
{
    [DefaultExecutionOrder(-11)]
    public class CoreLoader : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                if (!GameManager.HasInstance())
                    SceneManager.LoadScene("NueCore", LoadSceneMode.Additive);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError("You should add NueCore scene to build settings!");
                throw;
            }
           
        }
    }
}