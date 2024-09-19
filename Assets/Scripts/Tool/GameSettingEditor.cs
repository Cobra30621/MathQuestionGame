using Data.Encounter;
using Data.Settings;
using Enemy;
using Enemy.Data;
using NueGames.Data.Containers;
using NueGames.Data.Settings;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// Provides all settings data for game planner
    /// </summary>
    public class GameSettingEditor : MonoBehaviour
    {
        [Title("關卡設定")] 
        [Required] [InlineEditor] [LabelText("可選擇的玩家與關卡")]
        public AllyAndStageSetting AllyAndStageSetting;

        [Required] [InlineEditor] [LabelText("所有關卡")]
        public StageDataOverview StageDataOverview;

        
        
        [Title("敵人")]
        [Required] [InlineEditor] [LabelText("敵人遭遇設計")]
        public EnemyEncounterOverview EncounterOverview;
        
        [Required] [InlineEditor] [LabelText("敵人 Prefab")]
        public EnemyPrefabData EnemyPrefabData;
        
        
        [Title("基礎設定")] 
        [Required] [InlineEditor] [LabelText("遊戲基礎設定")]
        public GameplayData GameplayData;
        
        [Required] [InlineEditor] [LabelText("物品掉落量")]
        public ItemDropData ItemDropData;


        [Title("戰鬥相關 UI")] 
        [Required] [InlineEditor] [LabelText("能力說明資料")]
        public PowersData PowersData;
        
        [Required] [InlineEditor] [LabelText("遺物說明資料")]
        public RelicsData RelicsData;

        [Required] [InlineEditor] [LabelText("意圖說明資料")]
        public IntentionData IntentionData;
    }
}