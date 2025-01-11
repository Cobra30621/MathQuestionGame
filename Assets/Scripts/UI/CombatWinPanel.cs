using Save;
using UnityEngine;
using Utils;

namespace UI
{
    public class CombatWinPanel : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        public void BackToMenu()
        {
            SaveManager.Instance.ClearSingleGameData();
            sceneChanger.OpenMainMenuScene();
        }
    }
}