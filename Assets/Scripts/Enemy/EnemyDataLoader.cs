using System.Collections;
using Card;
using Enemy.EnemySkillInfo;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyDataLoader : MonoBehaviour
    {
        public EnemyName EnemyName;

        [InlineEditor]
        [LabelText("敵人資料")]
        public EnemyInfoOverview enemyInfoOverview;

        [LabelText("敵人回合行動資料")]
        [InlineEditor] public EnemyActionOverview enemyActionOverview;
        
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
            enemyActionOverview.ParseDataFromGoogleSheet();

            yield return new WaitUntil(()=>!enemyActionOverview.IsLoading);
            
            // 讀取敵人資料
            enemyInfoOverview.ParseDataFromGoogleSheet();
            
            yield return new WaitUntil(()=>!enemyInfoOverview.IsLoading);
            
            skillData.ParseDataFromGoogleSheet();
            
            yield return new WaitUntil(()=>!skillData.IsLoading);
            

            // 幫所有敵人設置對應的技能 ID
            enemyInfoOverview.SetEnemySkillInfo(enemyActionOverview);
        
            // 幫所有敵人行動設置技能 ID
            enemyActionOverview.SetSkillInfo(skillData);
            
        }

        public EnemyInfo.EnemyInfo EnemyInfo;
        
        [Button]
        public void PrintAllSkill()
        {
            EnemyInfo = enemyInfoOverview.FindUniqueId(EnemyName.Id);
            
        }
        
    }
}