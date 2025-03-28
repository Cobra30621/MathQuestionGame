using System.Collections.Generic;
using Map;

namespace Effect.Enemy
{
    /// <summary>
    /// 此类表示游戏中生成敌人的效果
    /// </summary>
    public class SpawnEnemyEffect : EffectBase
    {
        // 要生成的敌人数量
        private readonly int _spawnCount;
        // 要生成的敌人ID
        private List<string> _spawnEnemyIds;


        /// <summary>
        /// SpawnEnemyAction类的构造函数。
        /// 根据提供的技能信息初始化生成数量和敌人ID。
        /// </summary>
        /// <param name="skillInfo">包含生成敌人所需参数的技能信息。</param>
        public SpawnEnemyEffect(SkillInfo skillInfo)
        {
            _spawnCount = skillInfo.EffectParameterList[0];


            _spawnEnemyIds = new List<string>();
            for (int i = 1; i < skillInfo.EffectParameterList.Count; i++)
            {
                _spawnEnemyIds.Add($"{skillInfo.EffectParameterList[i]}");
            }
        }


        
        /// <summary>
        /// 执行生成敌人的主要动作。
        /// 访问角色处理器以构建指定数量和ID的敌人。
        /// </summary>
        public override void Play()
        {
            var characterHandler = CombatManager.characterHandler;

            for (int i = 0; i < _spawnCount; i++)
            {
                characterHandler.BuildEnemy(GetRandomId());
            }
        }

        private string GetRandomId()
        {
            return _spawnEnemyIds.Random();
        }
    }
}
