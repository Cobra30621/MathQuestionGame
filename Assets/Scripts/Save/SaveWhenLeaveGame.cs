using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save
{
    /// <summary>
    /// 當遊戲退出時儲存
    /// </summary>
    public class SaveWhenLeaveGame : MonoBehaviour
    {
        
        private void OnApplicationQuit()
        {
            // 如果正在遊戲中(非開頭介面)，需要儲存單局遊戲資料
            if (!IsMenuGameScene())
            {
                SaveManager.Instance.SaveSingleGame();
            }
            
            SaveManager.Instance.SavePermanentGame();
        }

        private bool IsMenuGameScene()
        {
            var currentScene = SceneManager.GetActiveScene().name;

            return currentScene == "0- Main Menu";
        }
    }
}