using System.Collections.Generic;
using Characters.Ally;
using Characters.Enemy;
using Characters.Enemy.Data;
using Log;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class CharacterHandler : MonoBehaviour
    {
        [SerializeField] private Transform allyPos;

        [SerializeField] private EnemyBuilder _enemyBuilder;

        // 所有敵人清單
        [ShowInInspector] public List<Enemy> Enemies { get; private set; }

        // 玩家
        public Ally MainAlly;


        [LabelText("效果生成敵人的特效")] [Required] public GameObject spawnEnemyFXPrefab;


        public Enemy RandomEnemy()
        {
            return Enemies.Random();
        }

        #region Build Characters

        public void BuildEnemies(List<EnemyName> enemyNames)
        {
            Enemies = new List<Enemy>();
            foreach (var enemyData in enemyNames)
            {
                var enemy = _enemyBuilder.Build(enemyData);
                

                Enemies.Add(enemy);
            }
        }

        /// <summary>
        /// 給效果創建敵人
        /// </summary>
        /// <param name="ids"></param>
        public void BuildEnemy(string id)
        {
            if (_enemyBuilder.ReachMaxEnemyCount()) return;

            var enemy = _enemyBuilder.Build(id);
            Enemies.Add(enemy);

            Instantiate(spawnEnemyFXPrefab, enemy.gameObject.transform);

            // 執行開始的行動
            StartCoroutine(enemy.BattleStartActionRoutine());
        }

        public void BuildAndSetEnemyHealth(string id, int health)
        {
            if (_enemyBuilder.ReachMaxEnemyCount()) return;

            var enemy = _enemyBuilder.Build(id);
            Enemies.Add(enemy);
            enemy.SetMaxHealth(health);
            

            // 執行開始的行動
            StartCoroutine(enemy.BattleStartActionRoutine());
        }

        public void BuildAllies(AllyData allyData)
        {
            var clone = Instantiate(allyData.prefab, allyPos);
            clone.BuildCharacter(allyData, this);
            MainAlly = clone;
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"創建玩家: {allyData.CharacterName}");
        }
        
        #endregion

        /// <summary>
        /// 取的指定 id 的敵人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool GetEnemyWithId(string id, out Enemy output)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.GetId() == id)
                {
                    output = enemy;
                    return true;
                }
            }

            output = null;
            return false;
        }


        public void OnAllyDeath(Ally targetAlly)
        {
            CombatManager.Instance.LoseCombat();
        }

        public void OnEnemyDeath(Enemy targetEnemy)
        {
            Enemies.Remove(targetEnemy);
            if (Enemies.Count <= 0)
                CombatManager.Instance.WinCombat();
        }
    }
}