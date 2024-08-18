namespace NueGames.Power
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
        Poison = 6,
        Fire = 7,
        // not official coded
        Weak = 1111,
        //






        None = 1000,
        // 傷害計算相關
        Block = 1001,




        
        BloodFury = 6,
        Hardcore = 7,
        Miss = 8,
        Shield = 9,
        Solid = 10,
        Bleed = 11,

        // 戰鬥相關
        Stun = 22,
        SelfDestruct = 23,

        // 卡牌與魔力相關
        MathMana = 41,
        GainManaAtRoundStart = 42,
        Overload = 43,

        // 能力相關
        ReduceStrengthPowerAtEndOfTurn = 61,


        // 測試用

        MathHappy = 131,
        MathAngry = 132,

        // 燃燒
        SuperFire = 202,
        Kindle = 203,
        GetGainManaPowerIfBeAttackByBurningEnemy = 211,

    }
}