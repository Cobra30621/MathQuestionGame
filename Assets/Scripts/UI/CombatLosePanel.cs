using NueGames.Utils;
using UnityEngine;

namespace NueGames.UI
{
    public class CombatLosePanel : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        public void BackToMenu()
        {
            SaveManager.Instance.ClearGameData();
            sceneChanger.OpenMainMenuScene();
        }
    }
}