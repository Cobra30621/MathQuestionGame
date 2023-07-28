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
        
        // 111~200 角色能力
        ApplyPower = 111, 
        MultiplyPower = 114,
        ClearPower = 115,
        ApplyBlock = 118,
        
        // 301~400 永久效果(結束戰鬥持續)
        
        
        // 401~500 數學系統
        EnterMathQuestioning = 401, 
        
    
        // 卡組相關 501 ~ 600
        EarnMana = 501,
        DrawCard = 502,
        Heal = 503,
        GainCardInThisBattle = 511,
        
        
        FX = 1031,
        
    }
}