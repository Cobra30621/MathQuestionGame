using NueGames.Utils;
using Save;
using UnityEngine;

namespace NueGames.UI
{
    public class CombatWinPanel : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        public void BackToMenu()
        {
            SaveManager.Instance.ClearGameData();
            sceneChanger.OpenMainMenuScene();
        }
    }
}