﻿namespace NueGames.Power
{
    /// <summary>
    /// 能力類型
    /// </summary>
    public enum PowerName
    {
        // 傷害計算相關
        Block = 1,
        Strength = 2,
        Vulnerable = 4,
        Weak = 5,
        BloodFury = 6,
        Hardcore = 7,
        Miss = 8,
        Shield = 9,
        
        // 戰鬥相關
        Stun = 22,
        
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
        Fire = 201,
        SuperFire = 202,
        Kindle = 203,
        GetGainManaPowerIfBeAttackByBurningEnemy = 211,
        
    }
}