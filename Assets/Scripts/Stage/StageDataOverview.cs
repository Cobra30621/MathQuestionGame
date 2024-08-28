using System.Collections.Generic;
using Data.Encounter;
using Map_System.Scripts.MapData;
using Newtonsoft.Json;
using NueGames.Data.Encounter;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Stage
{
    public class StageDataOverview : DataOverviewBase<StageDataOverview, StageData>
    {

        [Title("開發者工具")]
        
        [LabelText("複製用基底關卡")]
        public StageName blueprintStageName;

        
        [Button("將目標地圖複製到全部地圖")]
        public void CopyStage()
        {
            var blueprintStage = FindUniqueId(blueprintStageName.Id);

            string mapJson = JsonConvert.SerializeObject(blueprintStage.maps);
            Debug.Log(mapJson);

            foreach (var stageData in ids)
            {
                if (blueprintStage.name != stageData.name)
                {
                    Debug.Log($"Set {stageData.name} to {blueprintStage.name}");
                    stageData.maps = JsonConvert.DeserializeObject<List<MapConfig>>(mapJson);
                    CopyStageData(blueprintStage, stageData);
                
                    EditorUtility.SetDirty(stageData);
                }
            }
            
            
            AssetDatabase.SaveAssets();
        }


        private void CopyStageData(StageData origin, StageData target)
        {
            for (int i = 0; i < origin.maps.Count; i++)
            {
                var originMap = origin.maps[i];
                var targetMap = target.maps[i];
                
                CopyEncounterStage(originMap.encounterStage, targetMap.encounterStage);
                
            }
        }

        private void CopyEncounterStage(EncounterStage origin, EncounterStage target)
        {
            CopyEncounterList(origin.weakEnemies, target.weakEnemies);
            CopyEncounterList(origin.strongEnemies, target.strongEnemies);
            CopyEncounterList(origin.bossEnemies, target.bossEnemies);
            CopyEncounterList(origin.eliteEnemies, target.eliteEnemies);
            
            
        }

        public void CopyEncounterList(EnemyEncounterList origin, EnemyEncounterList target)
        {
            for (int i = 0; i < origin.weightClip.Count; i++)
            {
                target.weightClip[i].SetEncounter(origin.weightClip[i].Encounter);
            }
        }
    }
    
    
}