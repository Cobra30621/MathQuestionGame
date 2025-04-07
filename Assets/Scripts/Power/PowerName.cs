using Sirenix.OdinInspector;

namespace Power
{
    /// <summary>
    /// 能力類型
    /// </summary>
    public enum PowerName
    {
        // Buff
        [LabelText("反彈")]
        Thorns = 0,
        [LabelText("易傷")]
        Vulnerable = 1,
        [LabelText("力量")]
        Strength = 2,
        [LabelText("無敵")]
        Invincible = 3,
        [LabelText("狂怒")]
        Frenzy = 4,
        
        
        [LabelText("中毒")]
        Poison = 6,
        [LabelText("燃燒")]
        Fire = 7,
        [LabelText("惡魔獻祭")]
        DemonicSacrifice = 8,
        
        
        [LabelText("死後強化")]
        StrengthAfterDead = 11,
        [LabelText("盔甲")]
        Shield = 12,
        [LabelText("裝備")]
        Equip = 13,
        [LabelText("畜力")]
        Charge = 14,
        [LabelText("虛弱")]
        Weak = 15,
        [LabelText("不穩定")]
        SplitEnemy = 16,
        
        
        [LabelText("投資")]
        Investing = 20,
        [LabelText("循環")]
        Cycle = 21,
        
        [LabelText("未來魔力")]
        FutureMana = 23,
        [LabelText("未來護盾")]
        FutureBlock = 24,
        [LabelText("未來力量")]
        FutureStrength = 25,
        
        [LabelText("魔力泉湧")]
        ManaSprings = 26,
        [LabelText("魔力節省")]
        ManaSaving = 27,
        [LabelText("自毀")]
        SelfDestruct = 28,
       
        
        [LabelText("護盾")]
        Block = 1001,
        
        // not official coded
        Trap = 1112,
        //
        None = 1000,
        // 傷害計算相關
        

        // 能力相關
        ReduceStrengthPowerAtEndOfTurn = 61,
        
        // Test
        Test = 9999999
    }
}