using System.Collections.Generic;
using Characters;
using Combat;
using Economy;
using Effect;
using Effect.Damage;
using Effect.Parameters;
using Map;
using MapEvent;
using Save;
using Sirenix.OdinInspector;
using Stage;
using UI;
using UnityEngine;

namespace Tool
{
    public class TestTool : MonoBehaviour
    {


        [Button("更新事件")]
        public void UpdateEvent()
        {
            UIManager.Instance.EventCanvas.ShowEvent();
        }

        [Button("更新地圖")]
        public void UpdateMap()
        {
            MapManager.Instance.Initialized(StageSelectedManager.Instance.GetStageData());
        }
        
        [Button("獲得金錢")]
        public void AddMoney(int money)
        {
            CoinManager.Instance.AddCoin(money, CoinType.Money);
        }

        [Button("獲得寶石")]
        public void AddStone(int stone)
        {
            CoinManager.Instance.AddCoin(stone, CoinType.Stone);
        }

        [Button("擊敗敵人")]
        public void KillAllEnemy()
        {
            List<CharacterBase> targets = CombatManager.Instance.EnemiesForTarget();
            
            var damageInfo = new DamageInfo(999, new EffectSource(), fixDamage: true, canPierceArmor:true);

            EffectExecutor.AddEffect(new DamageEffect(damageInfo, targets));
        }

        [Button("清除存檔")]
        public void ClearSaveData()
        {
            SaveManager.Instance.ClearAllData();
        }

    }
}