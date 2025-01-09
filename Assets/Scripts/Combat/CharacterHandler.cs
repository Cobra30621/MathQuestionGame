using System.Collections.Generic;
using Enemy;
using Feedback;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.NueExtentions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class CharacterHandler : MonoBehaviour
    {

        public int MAX_ENEMY_COUNT = 4;
        
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private Transform allyPos;
        
        [SerializeField] private EnemyBuilder _enemyBuilder;
        
        // 所有敵人清單
        [ShowInInspector]
        public List<Enemy.Enemy> Enemies { get; private set; }
        // 玩家
        public Ally MainAlly;


        [LabelText("效果生成敵人的特效")]
        [Required]
        public GameObject spawnEnemyFXPrefab;

        
        public Enemy.Enemy RandomEnemy()
        {
            return Enemies.RandomItem();
        }

        #region Build Characters

        public void BuildEnemies(List<EnemyName> enemyNames)
        {
            Enemies = new List<Enemy.Enemy>();
            foreach (var enemyData in enemyNames)
            {
                var enemy = _enemyBuilder.Build(enemyData, GetEnemyPos());
                
                Enemies.Add(enemy);
            }
        }

        /// <summary>
        /// 給效果創建敵人
        /// </summary>
        /// <param name="ids"></param>
        public void BuildEnemy(string id)
        {
            if(ReachMaxEnemyCount()) return;

            var spawnPos = GetEnemyPos();
            var enemy = _enemyBuilder.Build(id, spawnPos);
            Enemies.Add(enemy);

            Instantiate(spawnEnemyFXPrefab, spawnPos);
            
            // 執行開始的行動
            StartCoroutine(enemy.BattleStartActionRoutine());
        }

        public void BuildAndSetEnemyHealth(string id, int health)
        {
            if(ReachMaxEnemyCount()) return;
            
            var enemy = _enemyBuilder.Build(id, GetEnemyPos());
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
        }
        

        private Transform GetEnemyPos()
        {
            if (!ReachMaxEnemyCount())
            {
                foreach (var enemyPos in enemyPosList)
                {
                    if(enemyPos.childCount == 0) return enemyPos;  
                }
                
            }
            return enemyPosList[0];
            
        }

        private bool ReachMaxEnemyCount()
        {
            bool isReachMax = Enemies.Count >= MAX_ENEMY_COUNT;
            if (isReachMax)
            {
                Debug.LogError($"敵人數量超過限制 {Enemies.Count + 1}");
            }

            return isReachMax;
        }

        #endregion

        /// <summary>
        /// 取的指定 id 的敵人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool GetEnemyWithId(string id,  out Enemy.Enemy output)
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
        public void OnEnemyDeath(Enemy.Enemy targetEnemy)
        {
            Enemies.Remove(targetEnemy);
            if (Enemies.Count<=0)
                CombatManager.Instance.WinCombat();
        }
        
        
    }
}