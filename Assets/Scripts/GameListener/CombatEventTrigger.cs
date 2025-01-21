using System;
using System.Collections.Generic;
using Combat;
using Managers;
using Relic;
using UnityEngine;

namespace GameListener
{
    /// <summary>
    /// 負責觸發各種 GameEventListener (遊戲事件的監聽者)
    /// 提供 CombatManger(戰鬥管理器) 觸發
    /// </summary>
    public static class CombatEventTrigger
    {
        #region 戰鬥流程

        /// <summary>
        /// 執行遊戲回合開始時，觸發的方法
        /// </summary>
        public static void InvokeOnRoundStart(RoundInfo info)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener != null)
                    listener.OnRoundStart(info);
            }
        }
        
        /// <summary>
        /// 執行遊戲回合結束時，觸發的方法
        /// </summary>
        public static void InvokeOnRoundEnd(RoundInfo info)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener != null)
                    listener.OnRoundEnd(info);
            }
        }
        
        /// <summary>
        /// 執行 玩家/敵人 階段開始時，觸發的方法
        /// </summary>
        public static void InvokeOnTurnStart(TurnInfo info)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener != null)
                    listener.OnTurnStart(info);
            }
        }
        
        /// <summary>
        /// 執行 玩家/敵人 階段結束時，觸發的方法
        /// </summary>
        public static void InvokeOnTurnEnd(TurnInfo info)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener != null)
                    listener.OnTurnEnd(info);
            }
        }


        /// <summary>
        /// 戰鬥開始時觸發
        /// </summary>
        public static void InvokeOnBattleStart()
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener!= null)
                    listener.OnBattleStart();
            }
        }
        
        /// <summary>
        /// 戰鬥勝利時觸發
        /// </summary>
        public static void InvokeOnBattleWin(int roundNumber)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener!= null)
                    listener.OnBattleWin(roundNumber);
            }
        }

        /// <summary>
        /// 戰鬥失敗時觸發
        /// </summary>
        public static void InvokeOnBattleLose(int roundNumber)
        {
            var listeners = ListenerGetter.GetAllEventsListeners();
            foreach (var listener in listeners)
            {
                if(listener!= null)
                    listener.OnBattleLose(roundNumber);
            }
        }
        

        #endregion
        
    }
}