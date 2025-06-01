using Sirenix.OdinInspector;

namespace NueGames.Enums
{
    // 遊戲玩家的職業
    public enum AllyClassType
    {  
        [LabelText("通用")]
        General = 0,
        [LabelText("騎士")]
        Knight = 1,
        [LabelText("獵人")]
        Hunter = 2,
        [LabelText("法師")]
        Mage = 3,
        [LabelText("盜賊")]
        Thief = 4,  
    }
}