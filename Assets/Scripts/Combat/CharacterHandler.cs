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
        public List<EnemyBase> Enemies { get; private set; }
        // 玩家
        public AllyBase MainAlly;


        [LabelText("效果生成敵人的特效")]
        [Required]
        public GameObject spawnEnemyFXPrefab;

        
        public EnemyBase RandomEnemy()
        {
            return Enemies.RandomItem();
        }

        #region Build Characters

        public void BuildEnemies(List<EnemyName> enemyNames)
        {
            Enemies = new List<EnemyBase>();
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
        }

        public void BuildAndSetEnemyHealth(string id, int health)
        {
            if(ReachMaxEnemyCount()) return;
            
            var enemy = _enemyBuilder.Build(id, GetEnemyPos());
            Enemies.Add(enemy);
            enemy.SetMaxHealth(health);
        }

        public void BuildAllies(AllyData allyData)
        {
            if(ReachMaxEnemyCount()) return;
            
            var clone = Instantiate(allyData.prefab, allyPos);
            clone.BuildCharacter(allyData, this);
            MainAlly = clone;
        }
        

        private Transform GetEnemyPos()
        {
            if (!ReachMaxEnemyCount())
            {
                return enemyPosList[Enemies.Count ];
            }
            else
            {
               
                return enemyPosList[0];
            }
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

        
        public void OnAllyDeath(AllyBase targetAlly)
        {
            CombatManager.Instance.LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            Enemies.Remove(targetEnemy);
            if (Enemies.Count<=0)
                CombatManager.Instance.WinCombat();
        }
        
        
    }
}