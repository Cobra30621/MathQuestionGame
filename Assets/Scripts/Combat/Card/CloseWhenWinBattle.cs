using UnityEngine;

namespace Combat.Card
{
    /// <summary>
    /// 給 HandController 用的，在戰鬥結束後關閉手牌
    /// </summary>
    public class CloseWhenWinBattle : MonoBehaviour
    {
        private void Awake()
        {
            // Register Events
            CombatManager.OnBattleWin += CloseWhenWin;
        }

        private void OnDestroy()
        {
            CombatManager.OnBattleWin -= CloseWhenWin;
        }

        private void CloseWhenWin(int round)
        {
            gameObject.SetActive(false);
        }
    }
}