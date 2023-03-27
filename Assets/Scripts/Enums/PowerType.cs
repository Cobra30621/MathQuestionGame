namespace NueGames.Enums
{
    public enum PowerType
    {
        None =0,
        // 傷害計算相關
        Block = 1,
        Strength = 2,
        Dexterity = 3,
        Vulnerable = 4,
        Weak = 5,
        
        // 戰鬥相關
        Poison = 21,
        Stun = 22,
        
        // 卡牌與魔力相關
        MathMana = 41,
        
        // 每回合給予
        Angry = 51,
        
        // 遺物、角色(暫時)
        Character_MathManaCharacter = 201,
        Relic_DrawCardOnAnswerCorrect = 202
    }
}