namespace Action.Card
{
    public class ChanceAction : GameActionBase
    {
        // 本回合已抽卡次數
        private int _drawCardCount;
        // 要抽的卡片數
        private int _times;
        
        public ChanceAction(int drawCardCount, int times)
        {
            _drawCardCount = drawCardCount;
            _times = times;
        }
        
        protected override void DoMainAction()
        {
            _times = 2;
            // 判斷是否已經抽過卡
            _drawCardCount = 1;
            if (_drawCardCount == 0) _times += 1;
            var drawCardAction = new DrawCardAction(_times, ActionSource);
            drawCardAction.DoAction();
        }
    }
}