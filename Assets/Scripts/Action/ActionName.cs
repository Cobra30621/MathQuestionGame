namespace NueGames.Action
{
    /// <summary>
    /// 遊戲行為類型
    /// </summary>
    public enum ActionName
    {
        // 1~10 傷害
        Damage = 1, 
        DamageByQuestioning = 4,
        DamageByAllyPowerValue = 5,
        DamageByAnswerCountInThisBattle = 6,
        DamageAndTriggerActionIfDamageSuccess = 7,
        
        // 111~200 角色能力
        ApplyPower = 111, 
        ApplyPowerToAllEnemy = 113,
        MultiplyPower = 114,
        ClearPower = 115,
        ApplyPowerByMathManaValue = 117,
        ApplyBlock = 118,
        ApplyMathMana = 119,
        
        // 301~400 永久效果(結束戰鬥持續)
        IncreaseMaxHealth = 301,  
        
        
        // 401~500 數學系統
        EnterMathQuestioning = 401, 
        
    
        // 卡組相關 501 ~ 600
        EarnMana = 501,
        DrawCard = 502,
        Heal = 503,
        GainCardInThisBattle = 511,
        
        // 其他系統 1001~
        UseRandom = 1011, // 隨機行動
        // 71 法力系統
        UnlimitedUseMathManaCard = 1051,
    }

    /// <summary>
    /// 答題後觸發行為的類型
    /// </summary>
    public enum CardActionDataListType
    {
        Normal,
        Correct,
        Wrong
    }
}