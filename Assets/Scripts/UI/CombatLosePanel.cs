using Managers;
using Save;
using UnityEngine;
using Utils;

namespace UI
{
    public class CombatLosePanel : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        public void BackToMenu()
        {
            SaveManager.Instance.ClearSingleGameData();
            GameManager.Instance.ExitSingleGame();
        }
    }
}