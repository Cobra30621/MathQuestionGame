using Sirenix.OdinInspector;

namespace Effect.Parameters
{
    /// <summary>
    /// 定義各種效果（Effect）的唯一識別與顯示文字格式
    /// </summary>
    public enum EffectName
    {
        /// <summary>對目標造成指定點數的傷害，重複次數由參數 [2] 決定</summary>
        [LabelText("造成[1]點傷害[2]次")]
        Damage = 20001,

        /// <summary>提升目標指定點數的護盾，重複次數由參數 [2] 決定</summary>
        [LabelText("提昇目標[1]點護盾[2]次")]
        ApplyBlock = 20002,

        /// <summary>給予目標指定層數的狀態，[1] 為狀態編號，[2] 為層數</summary>
        [LabelText("給予目標[1]狀態[2]層")]
        ApplyPower = 20003,

        /// <summary>為目標恢復指定點數的血量</summary>
        [LabelText("恢復目標[1]點血量")]
        Heal = 20004,

        /// <summary>向目標卡堆中加入指定數量的卡牌</summary>
        [LabelText("給予目標[1]卡牌共[2]張")]
        AddCardToPile = 20005,

        /// <summary>造成怪物生命總合的傷害，[1] 決定傷害次數</summary>
        [LabelText("造成怪物生命總合的傷害[1]次")]
        EnemyHpDamage = 20006,

        /// <summary>回復指定點數的魔力</summary>
        [LabelText("回復[1]點魔力")]
        GainMana = 20007,

        /// <summary>根據參數 [1] 對目標造成護盾，點數由參數 [2] 決定</summary>
        [LabelText("根據[1]對目標造成[2]點護盾")]
        BlockByCount = 20009,

        /// <summary>根據參數 [1] 對目標造成傷害，點數由參數 [2] 決定</summary>
        [LabelText("根據[1]對目標造成[2]點傷害")]
        DamageByCount = 20010,

        /// <summary>從牌組抽取指定張數的卡片</summary>
        [LabelText("從牌組抽 [1]張卡片")]
        DrawCard = 20021,

        /// <summary>清除目標身上的指定狀態</summary>
        [LabelText("清除 [1] 的狀態")]
        ClearPower = 20031,

        /// <summary>符合條件時抽更多卡片，[1]張基本數，[3]張額外抽卡條件</summary>
        [LabelText("從牌組抽 [1] 張卡，符合條件 [2] 時 ，多抽[3]張卡")]
        DrawMoreCardWhenCondition = 20041,

        /// <summary>符合條件時獲得更多護盾，[1]點基本護盾，[3]點額外護盾條件</summary>
        [LabelText("獲得 [1] 點格檔，符合條件 [2] 時 ，多獲得 [2] 點格檔")]
        AddMoreBlockWhenCondition = 20042,

        /// <summary>符合條件時增加魔力，[1]點基本魔力，[2]條件觸發後增量</summary>
        [LabelText("當符合 [1] 條件時，增加 [2] 點魔力")]
        AddManaWhenCondition = 20043,

        /// <summary>當使用足夠張數的卡片後，造成更高傷害；[1]基本傷害，[2]觸發條件張數，[3]觸發後傷害</summary>
        [LabelText("造成 [1] 點傷害，如果本回合使用過 [2] 張以上的卡片，改造成 [3] 點傷害")]
        DamageMoreWhenUseEnoughCard = 20051,

        /// <summary>結束玩家當前回合</summary>
        [LabelText("結束玩家的回合")]
        EndPlayerTurn = 20100,

        /// <summary>符合條件時，執行另一個 EffectID；參數 [1] 為條件，[2] 為要執行的 EffectID，[3~n] 為該效果參數</summary>
        [LabelText("當符合 [1] 條件時，執行EffectId [2]的效果，該效果的參數 [3 ~ n] ")]
        DoEffectWhenCondition = 90001,

        // 敵人專用效果
        /// <summary>在場上生成指定敵人</summary>
        [LabelText("生成敵人")]
        SpawnEnemy = 30001,

        /// <summary>對指定敵人執行分裂行為</summary>
        [LabelText("分裂")]
        SplitEnemy = 30002,

        /// <summary>消耗自身或其他資源以進行惡魔獻祭</summary>
        [LabelText("惡魔獻祭")]
        DemonicSacrifice = 30003,
    }
}
