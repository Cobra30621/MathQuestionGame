using NueGames.Card;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Relic;

namespace NueGames.Combat
{
    public class DamageInfo
    {
        /// <summary>
        /// 傷害來源
        /// </summary>
        /// <returns></returns>
        public ActionSource ActionSource;
        /// <summary>
        /// 傷害對象
        /// </summary>
        public CharacterBase Target;
        /// <summary>
        /// 傷害數值
        /// </summary>
        public int Value;
        /// <summary>
        /// 固定傷害，不受狀態影響
        /// </summary>
        public bool FixDamage;
        /// <summary>
        /// 可以穿甲
        /// </summary>
        public bool CanPierceArmor;

        #region 選填

        /// <summary>
        /// 傷害源自哪個角色
        /// </summary>
        public CharacterBase Self;
        /// <summary>
        /// 傷害源自哪一個能力
        /// </summary>
        public PowerType SourcePower;
        /// <summary>
        /// 傷害源自哪一個遺物
        /// </summary>
        public RelicType SourceRelic;

        #endregion

        public DamageInfo()
        {
        }

        public DamageInfo(ActionParameters parameters)
        {
            ActionSource = parameters.ActionSource;
            Target = parameters.Target;
            Value = parameters.Value;
            FixDamage = false; // TODO
            CanPierceArmor = false;
            
            Self = parameters.Self;
            SourcePower = parameters.SourcePower;
            SourceRelic = parameters.SourceRelic;
        }
    }
}