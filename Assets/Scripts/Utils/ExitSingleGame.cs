using Managers;
using UnityEngine;

namespace Utils
{
    public class ExitSingleGame : MonoBehaviour
    {
        public void ExitGame()
        {
            GameManager.Instance.ExitSingleGame();
        }
    }
}