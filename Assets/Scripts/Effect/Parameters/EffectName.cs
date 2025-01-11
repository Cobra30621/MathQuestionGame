using Sirenix.OdinInspector;

namespace Effect.Parameters
{
    public enum EffectName
    {
        [LabelText("造成[1]點傷害 1 次")]
        Damage = 1,
        [LabelText("造成[1]點傷害[2]次")]
        MultiDamage = 20001,
        [LabelText("提昇目標[1]點護盾[2]次")]
        MultiBlock = 20002,
        [LabelText("給予目標[1]狀態[2]層")]
        ApplyPower = 20003,
        [LabelText("恢復目標[1]點血量")]
        Heal = 20004,
        [LabelText("給予目標[1]卡牌共[2]張")]
        AddCard = 20005,
        [LabelText("造成怪物生命總合的傷害[1]次")]
        EnemyHpDamage = 20006,
        [LabelText("回復[1]點魔力")]
        GainMana = 20007,
        [LabelText("根據[1]對目標造成[2]點護盾")]
        BlockByCount = 20009,
        [LabelText("根據[1]對目標造成[2]點傷害")]
        DamageByCount = 20010,
        RemoveCardFromPile = 1113,
        
        // Enemy
        [LabelText("生成敵人")]
        SpawnEnemy = 30001,
        [LabelText("分裂")]
        SplitEnemy = 30002,
        [LabelText("惡魔獻祭")]
        DemonicSacrifice = 30003,
        [LabelText("給予卡牌到牌堆")]
        AddCardToPile = 30010,
    }
}