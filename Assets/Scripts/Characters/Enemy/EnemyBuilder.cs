using System.Collections.Generic;
using Characters.Enemy.Data;
using Combat;
using Log;
using Sheets;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyBuilder : MonoBehaviour
    {
        public SheetDataGetter sheetDataGetter;

        public EnemyPrefabData enemyPrefabData;

        public CharacterHandler characterHandler;

        public int MAX_ENEMY_COUNT = 4;
        [SerializeField] private List<Transform> enemyPosList;
        
        public Enemy Build(string id)
        {
            var enemyData =sheetDataGetter.enemyData.FindUniqueId(id);
            return Build(enemyData);
        }

        public Enemy Build(EnemyName enemyName)
        {
            var enemyData = sheetDataGetter.enemyData.FindUniqueId(enemyName.Id);
            return Build(enemyData);
        }
        
        public Enemy Build(EnemyData data)
        {
            var prefab = enemyPrefabData.GetPrefab(data.Prefab);
            int posIndex = GetEnemyPos();
            var clone = Instantiate(prefab, enemyPosList[posIndex]);
            
            clone.BuildCharacter(data, sheetDataGetter, characterHandler);
            clone.name = clone.name + $"-{posIndex}";

            EventLogger.Instance.LogEvent(LogEventType.Combat, $"創建敵人: {clone.name} {clone.GetId()}");
            
            return clone;
        }
        
        
        private int GetEnemyPos()
        {
            if (!ReachMaxEnemyCount())
            {
                for (var index = 0; index < enemyPosList.Count; index++)
                {
                    var enemyPos = enemyPosList[index];
                    if (enemyPos.childCount == 0) return index;
                }
            }

            return 0;
        }

        public bool ReachMaxEnemyCount()
        {
            bool isReachMax = characterHandler.Enemies.Count >= MAX_ENEMY_COUNT;
            if (isReachMax)
            {
                EventLogger.Instance.LogEvent(LogEventType.Combat, $"錯誤 - 敵人數量超過限制 {MAX_ENEMY_COUNT}");
            }

            return isReachMax;
        }
        
    }
}