namespace NueGames.Enums
{
    /// <summary>
    /// 遊戲行為的目標對象
    /// </summary>
    public enum ActionTargetType
    {
        Enemy = 1,
        Ally = 0,
        AllEnemies = 2,
        RandomEnemy,
        WithoutTarget
    }
}