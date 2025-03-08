using Sirenix.OdinInspector;

namespace Power
{
    /// <summary>
    /// 能力類型
    /// </summary>
    public enum PowerName
    {
        // Buff
        Thorns = 0,
        Vulnerable = 1,
        Strength = 2,
        Invincible = 3,
        Frenzy = 4,
        Poison = 6,
        Fire = 7,
        [LabelText("惡魔獻祭")]
        DemonicSacrifice = 8,
        
        [LabelText("不穩定")]
        SplitEnemy = 16,
        
        StrengthAfterDead = 11,
        Shield = 12,
        Equip = 13,
        Charge = 14,
        Weak = 15,
        
        [LabelText("本局對戰中，所有的卡牌消耗變為1")]
        ManaCostTo1 = 31,
       
        
        // not official coded
        Trap = 1112,
        //
        None = 1000,
        // 傷害計算相關
        Block = 1001,

        // 能力相關
        ReduceStrengthPowerAtEndOfTurn = 61,
        
        // Test
        Test = 9999999
    }
}