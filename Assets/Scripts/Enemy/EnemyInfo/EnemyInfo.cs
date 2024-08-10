using System;
using System.Collections.Generic;
using Enemy.EnemySkillInfo;
using rStarTools.Scripts.StringList;
using Tool;
using UnityEditor;
using UnityEngine;

namespace Enemy.EnemyInfo
{
    [Serializable]
    public class EnemyInfo : DataBase<EnemyInfoOverview>
    {
        public string ID;
        public string Lang;
        public string MobSkillID;
        public string Level;

        public List<EnemySkillInfo.EnemyAction> EnemySkillInfos;

        public void SetEnemySkill(EnemyActionOverview enemyInfoOverview)
        {
            var stringToList = Helper.ConvertStringToList(MobSkillID);

            EnemySkillInfos = new List<EnemySkillInfo.EnemyAction>();
            foreach (var i in stringToList)
            {
                
                var enemySkillInfo = enemyInfoOverview.FindUniqueId($"{i}");
                Debug.Log($"{i} {enemySkillInfo.ps}");
                EnemySkillInfos.Add(enemySkillInfo);
            }
        }
        
    }
    
}