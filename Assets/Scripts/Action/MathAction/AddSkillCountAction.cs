using NueGames.CharacterAbility;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 新增玩家使用職業天賦的次數
    /// </summary>
    public class AddSkillCountAction : GameActionBase
    {
        public int addCount;

        public AddSkillCountAction(int addCount)
        {
            this.addCount = addCount;
        }

        protected override void DoMainAction()
        {
            CharacterSkillManager.Instance.AddSkillCount(addCount);
        }
    }
}