using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 增加最大生命值
    /// </summary>
    public class IncreaseMaxHealthAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.IncreaseMaxHealth;

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                PlaySpawnTextFx(AdditionValue.ToString(), target);
                target.CharacterStats.IncreaseMaxHealth(AdditionValue);
            }
        }
    }
}