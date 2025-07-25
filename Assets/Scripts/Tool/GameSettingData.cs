using Card;
using Card.Data;
using Characters.Ally;
using Characters.Enemy.Data;
using Effect;
using Encounter.Data;
using Map;
using MapEvent;
using NueGames.Data.Containers;
using NueGames.Data.Settings;
using NueGames.Enums;
using Power;
using Question.Data;
using Relic.Data;
using Reward.Data;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;
using VersionControl;

namespace Tool
{
    public class GameSettingData : ScriptableObject
    {
        [Title("基礎數值")] 
        [Required] [InlineEditor] [LabelText("遊戲基礎設定")]
        public GameplayData GameplayData;
        
        [Required] [InlineEditor] [LabelText("物品掉落量")]
        public ItemDropData ItemDropData;

        [Title("版本")] 
        [Required] [InlineEditor] [LabelText("遊戲版本資料")]
        public GameVersionData GameVersionData;
        
        [Title("關卡設定")] 
        [Required] [InlineEditor] [LabelText("可選擇的玩家與關卡")]
        public AllyAndStageSetting AllyAndStageSetting;

        [Required] [InlineEditor] [LabelText("管理所有關卡")]
        public StageDataOverview StageDataOverview;

        [Required] [InlineEditor] [LabelText("設定地圖中的事件")]
        public EventList EventList;

        [Required] [InlineEditor] [LabelText("敵人遭遇(一場戰鬥)")]
        public EnemyEncounterOverview EncounterOverview;
        
        [Title("角色")]
        [Required] [InlineEditor] [LabelText("玩家資料(包含卡組)")]
        public AllyDataOverview AllyDataOverview;
        
        [Required] [InlineEditor] [LabelText("敵人 Prefab")]
        public EnemyPrefabData EnemyPrefabData;
        
        
        [Title("卡牌")] 
        [Required] [InlineEditor] [LabelText("正式版遊戲的卡片")]
        public DeckData officialDeck;
        
        [Required] [InlineEditor] [LabelText("開發者卡片")]
        public DeckData developDeck;

        [Required] [InlineEditor] [LabelText("卡牌資料管理")]
        public CardDataOverview CardDataOverview;
        
        [Title("設定與說明")] 
        [Required] [InlineEditor] [LabelText("能力說明資料")]
        public PowersData PowersData;
        
        [Required] [InlineEditor] [LabelText("遺物說明資料")]
        public RelicsData RelicsData;

        [Required] [InlineEditor] [LabelText("意圖說明資料")]
        public IntentionData IntentionData;

        [Required] [InlineEditor] [LabelText("設定獎勵介面的資訊")]
        public RewardContainerData RewardContainerData;

        [Required] [InlineEditor] [LabelText("角色卡牌圖片")]
        public CharacterCardSprites CharacterCardSprites;

        [Required] [InlineEditor] [LabelText("地圖節點小 Icon")]
        public NodeBlueprintData NodeBlueprintData;

        [Required] [InlineEditor] [LabelText("一些關鍵字的詞條(目前暫時無用)")]
        public SpecialKeywordData SpecialKeywords;

        [Title("答題")] 
        [Required] [InlineEditor] [LabelText("本地端的題目資料")]
        public QuestionData LocalQuestionData;

        [Required] [InlineEditor] [LabelText("答對題目數量與對應掉落寶石金額的對照表")]
        public QuestionStoneDropTable QuestionStoneDropTable;
        
        [Title("讀表相關")] 
        [Required] [InlineEditor] [LabelText("存檔用卡組")]
        public CardLevelData CardLevelData;
        
        [Required] [InlineEditor] [LabelText("技能資訊管理")]
        public SkillData SkillData;

        [Required] [InlineEditor] [LabelText("敵人資訊管理")]
        public EnemyDataOverview EnemyDataOverview;

        [Required] [InlineEditor] [LabelText("敵人每回合意圖資訊管理")]
        public EnemyIntentionData EnemyIntentionData;
    }
}