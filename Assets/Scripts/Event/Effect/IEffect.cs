using System;

namespace  NueGames.Event.Effect
{
    /// <summary>
    /// 定义可执行和检查是否可选择的效果接口
    /// </summary>
    public interface IEffect
    {
        void Init(EffectData effectData);
        
        void Execute(System.Action onComplete);
        bool IsSelectable();
    }
}