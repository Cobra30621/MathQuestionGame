using Card;
using Enemy.Data;
using Tool;
using UnityEngine;

namespace Enemy
{
    public class EnemyBuilder : MonoBehaviour
    {
        public SheetDataGetter sheetDataGetter;

        public EnemyPrefabData enemyPrefabData;


        public EnemyBase Build(EnemyName enemyName, Transform spawnPos)
        {
            var enemyData = sheetDataGetter.enemyDataOverview.FindUniqueId(enemyName.Id);
            return Build(enemyData, spawnPos);
        }
        
        public EnemyBase Build(EnemyData data, Transform spawnPos)
        {
            var prefab = enemyPrefabData.GetPrefab(data.Prefab);
            var clone = Instantiate(prefab, spawnPos);
            
            clone.BuildCharacter(data, sheetDataGetter);

            return clone;
        }
        
        
        
        
    }
}