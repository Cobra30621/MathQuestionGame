using NueGames.Utils;
using Save;
using UnityEngine;

namespace NueGames.UI
{
    public class CombatLosePanel : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        public void BackToMenu()
        {
            SaveManager.Instance.ClearSingleGameData();
            sceneChanger.OpenMainMenuScene();
        }
    }
}