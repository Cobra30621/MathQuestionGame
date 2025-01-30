using UnityEngine;

namespace Save
{
    /// <summary>
    /// 當遊戲退出時儲存
    /// </summary>
    public class SaveWhenLeaveGame : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            SaveManager.Instance.SaveSingleGame();
            SaveManager.Instance.SavePermanentGame();
        }
    }
}