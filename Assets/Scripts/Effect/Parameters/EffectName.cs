using Sirenix.OdinInspector;

namespace Effect.Parameters
{
    public enum EffectName
    {
        [LabelText("造成[1]點傷害[2]次")]
        Damage = 20001,
        [LabelText("提昇目標[1]點護盾[2]次")]
        ApplyBlock = 20002,
        [LabelText("給予目標[1]狀態[2]層")]
        ApplyPower = 20003,
        [LabelText("恢復目標[1]點血量")]
        Heal = 20004,
        [LabelText("給予目標[1]卡牌共[2]張")]
        AddCardToPile = 20005,
        [LabelText("造成怪物生命總合的傷害[1]次")]
        EnemyHpDamage = 20006,
        [LabelText("回復[1]點魔力")]
        GainMana = 20007,
        [LabelText("根據[1]對目標造成[2]點護盾")]
        BlockByCount = 20009,
        [LabelText("根據[1]對目標造成[2]點傷害")]
        DamageByCount = 20010,
        
        [LabelText("從牌組抽 [1]張卡片")]
        DrawCard = 20021,
        
        
        [LabelText("清除 [1] 的狀態")]
        ClearPower = 20031,
        
        
        [LabelText("從牌組抽 [1] 張卡，符合條件 [2] 時 ，多抽[3]張卡")]
        DrawMoreCardWhenCondition = 20041,
        [LabelText("獲得 [1] 點格檔，符合條件 [2] 時 ，多獲得 [2] 點格檔")]
        AddMoreBlockWhenCondition = 20042,
        [LabelText("當符合 [1] 條件時，增加 [2] 點魔力")]
        AddManaWhenCondition = 20043,
        
        [LabelText("造成 [1] 點傷害，如果本回合使用過 [2] 張以上的卡片，多造成 [3] 點傷害")]
        DamageMoreWhenUseEnoughCard = 20051,
        
        [LabelText("結束玩家的回合")]
        EndPlayerTurn = 20100,
        
        [LabelText("當符合 [1] 條件時，執行EffectId [2]的效果，該效果的參數 [3 ~ n] ")]
        DoEffectWhenCondition = 90001,
        
        
        // Enemy
        [LabelText("生成敵人")]
        SpawnEnemy = 30001,
        [LabelText("分裂")]
        SplitEnemy = 30002,
        [LabelText("惡魔獻祭")]
        DemonicSacrifice = 30003,
    }
}