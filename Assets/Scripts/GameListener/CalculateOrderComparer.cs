using System;
using System.Collections.Generic;
using NueGames.Power;

namespace GameListener
{
    public enum CalculateOrder
    {
        None = 0,
        AdditionAndSubtraction = 5, // 加減
        MultiplyAndDivide = 10, // 乘除
        DirectChange = 15, // 直接改變數值
        FinalChange = 20 // 最終直接改變
    }
    
    /// <summary>
    /// 用來排序 GameEventListener (如能力、遺物)的計算順序
    /// </summary>
    public class CalculateOrderClip
    {
        public CalculateOrderClip(CalculateOrder calculateOrder, Func<float, float> calculateFunction)
        {
            CalculateOrder = calculateOrder;
            CalculateFunction = calculateFunction;
        }


        public readonly CalculateOrder CalculateOrder;
        /// <summary>
        /// 計算的函數
        /// </summary>
        public Func<float,float> CalculateFunction;

    }
    
    /// <summary>
    /// 排序器，依據 CalculateOrder 屬性進行排序
    /// </summary>
    public class CalculateOrderComparer : IComparer<CalculateOrderClip>
    {
        public int Compare(CalculateOrderClip x, CalculateOrderClip y)
        {
            // 根據 CalculateOrder 屬性進行排序
            return x.CalculateOrder.CompareTo(y.CalculateOrder);
        }
    }
}