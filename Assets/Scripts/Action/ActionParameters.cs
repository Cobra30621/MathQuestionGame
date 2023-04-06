using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Card
{
    /// <summary>
    /// 遊戲行為(GameAction)所需參數
    /// </summary>
    public class ActionParameters
    {
        /// <summary>
        /// 行為數值
        /// </summary>
        public readonly int Value;
        /// <summary>
        /// 行為目標對象
        /// </summary>
        public readonly CharacterBase TargetCharacter;
        /// <summary>
        /// 行為產生者
        /// </summary>
        public readonly CharacterBase SelfCharacter;
        /// <summary>
        /// 行為資料
        /// </summary>
        public readonly ActionData ActionData;
        /// <summary>
        /// 卡片資料(卡牌行為才需要)
        /// </summary>
        public readonly CardData CardData;
        
        public ActionParameters(int value,CharacterBase target, CharacterBase self,ActionData actionData, CardData cardData)
        {
            Value = value;
            TargetCharacter = target;
            SelfCharacter = self;
            ActionData = actionData;
            CardData = cardData;
        }
    }
}