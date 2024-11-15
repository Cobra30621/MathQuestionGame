using Sirenix.OdinInspector;

namespace Action
{
    public enum GameActionType
    {
        Damage = 1,
        
        
        MultiDamage = 20001,
        MultiBlock = 20002,
        SuperDamage = 3,
        ApplyPower = 20003,
        EnemyHpDamage = 20006,
        GainMana = 20007,
        BlockByCount = 20009,
        DamageByCount = 20010,
        // not official coded action start from 111
        Flee = 1111,
        RemoveCardFromPile = 1113,
        
        EnemyBlock = 12,
        Block= 2,
        
        
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