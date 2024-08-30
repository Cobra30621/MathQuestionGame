using System.Collections.Generic;
using Enemy;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.NueExtentions;
using UnityEngine;

namespace Combat
{
    public class CharacterHandler : MonoBehaviour
    {
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private Transform allyPos;
        
        [SerializeField] private EnemyBuilder _enemyBuilder;
        
        // 所有敵人清單
        public List<EnemyBase> Enemies { get; private set; }
        // 玩家
        public AllyBase MainAlly;


        
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

        public void BuildAllies(AllyData allyData)
        {
            var clone = Instantiate(allyData.prefab, allyPos);
            clone.BuildCharacter(allyData, this);
            MainAlly = clone;
        }
        

        private Transform GetEnemyPos()
        {
            if (enemyPosList.Count > Enemies.Count)
            {
                return enemyPosList[Enemies.Count ];
            }
            else
            {
                Debug.LogError($"敵人數量超過限制{Enemies.Count + 1}");
                return enemyPosList[0];
            }
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