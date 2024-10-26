using Card;
using NueGames.Action;

namespace Action.Enemy
{
    /// <summary>
    /// 分裂敌人的动作类,用于生成新的敌人
    /// </summary>
    public class SplitEnemyAction : GameActionBase
    {
        /// <summary>
        /// 要生成的敌人ID
        /// </summary>
        private readonly string _spawnEnemyId;

        /// <summary>
        /// 生成的敌人的生命值
        /// </summary>
        private readonly int _health;

        /// <summary>
        /// 构造函数,通过技能信息初始化
        /// </summary>
        /// <param name="skillInfo">技能信息</param>
        public SplitEnemyAction(SkillInfo skillInfo)
        {
            _spawnEnemyId = $"{skillInfo.EffectParameterList[0]}";
        }
        
        /// <summary>
        /// 构造函数,直接指定敌人ID和生命值
        /// </summary>
        /// <param name="spawnEnemyId">要生成的敌人ID</param>
        /// <param name="health">生成的敌人的生命值</param>
        public SplitEnemyAction(string spawnEnemyId, int health)
        {
            _spawnEnemyId = spawnEnemyId;
            _health = health;
        }
        
        /// <summary>
        /// 执行主要动作,生成新的敌人并设置其生命值
        /// </summary>
        protected override void DoMainAction()
        {
            var characterHandler = CombatManager.characterHandler;

            // 生成新的敌人并设置其生命值
            characterHandler.BuildAndSetEnemyHealth(_spawnEnemyId, _health);
        }
    }
}
