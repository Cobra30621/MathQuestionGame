using System.Collections;
using Card;
using Enemy.Data;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyDataLoader : MonoBehaviour
    {
        public EnemyName EnemyName;

        [FormerlySerializedAs("enemyInfoOverview")]
        [InlineEditor]
        [LabelText("敵人資料")]
        public EnemyDataOverview enemyDataOverview;

        [FormerlySerializedAs("enemySkillInfoOverview")]
        [FormerlySerializedAs("enemyActionOverview")]
        [LabelText("敵人回合行動資料")]
        [InlineEditor] public EnemySkillDataOverview enemySkillDataOverview;
        
        [LabelText("技能資料")]
        [InlineEditor] public SkillData skillData;


        [Button]
        public void Load()
        {

            StartCoroutine(LoadCoroutine());
            
        }

        private IEnumerator LoadCoroutine()
        {
            // 讀取敵人技能
            enemySkillDataOverview.ParseDataFromGoogleSheet();

            yield return new WaitUntil(()=>!enemySkillDataOverview.IsLoading);
            
            // 讀取敵人資料
            enemyDataOverview.ParseDataFromGoogleSheet();
            
            yield return new WaitUntil(()=>!enemyDataOverview.IsLoading);
            
            skillData.ParseDataFromGoogleSheet();
            
            yield return new WaitUntil(()=>!skillData.IsLoading);
            
        }

        [FormerlySerializedAs("EnemyInfo")] public EnemyData enemyData;
        
        [Button]
        public void PrintAllSkill()
        {
            enemyData = enemyDataOverview.FindUniqueId(EnemyName.Id);
            
        }
        
    }
}