namespace Combat
{
    /// <summary>
    /// 遊戲行為的目標對象類型，用於指定技能或效果的作用對象
    /// </summary>
    public enum ActionTargetType
    {
        /// <summary>
        /// 指定的單一敵人
        /// </summary>
        SpecifiedEnemy = 1,

        /// <summary>
        /// 我方單一角色
        /// </summary>
        Ally = 0,

        /// <summary>
        /// 所有敵人
        /// </summary>
        AllEnemies = 2,

        /// <summary>
        /// 隨機一名敵人
        /// </summary>
        RandomEnemy = 3,

        /// <summary>
        /// 無特定目標（通常用於全域或被動效果）
        /// </summary>
        WithoutTarget = 4
    }
}