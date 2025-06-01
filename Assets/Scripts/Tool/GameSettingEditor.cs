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

namespace Tool
{
    /// <summary>
    /// Provides all settings data for game planner
    /// </summary>
    public class GameSettingEditor : MonoBehaviour
    {
        [Required] [InlineEditor] [LabelText("遊戲可設定資料")]
        public GameSettingData gameSettingData;
    }
}