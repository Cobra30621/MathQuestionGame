using Card;
using Combat;
using Enemy.Data;
using Sheets;
using Tool;
using UnityEngine;

namespace Enemy
{
    public class EnemyBuilder : MonoBehaviour
    {
        public SheetDataGetter sheetDataGetter;

        public EnemyPrefabData enemyPrefabData;

        public CharacterHandler characterHandler;

        public Enemy Build(string id, Transform spawnPos)
        {
            var enemyData =sheetDataGetter.enemyData.FindUniqueId(id);
            return Build(enemyData, spawnPos);
        }

        public Enemy Build(EnemyName enemyName, Transform spawnPos)
        {
            var enemyData = sheetDataGetter.enemyData.FindUniqueId(enemyName.Id);
            return Build(enemyData, spawnPos);
        }
        
        public Enemy Build(EnemyData data, Transform spawnPos)
        {
            var prefab = enemyPrefabData.GetPrefab(data.Prefab);
            var clone = Instantiate(prefab, spawnPos);
            
            clone.BuildCharacter(data, sheetDataGetter, characterHandler);

            return clone;
        }
        
    }
}