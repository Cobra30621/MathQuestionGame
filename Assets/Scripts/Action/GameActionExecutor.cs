using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Action.Parameters;
using Action.Sequence;
using DG.Tweening;
using Managers;
using NueGames.Action;
using NueGames.Parameters;
using NueGames.Power;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace NueGames.Managers
{
    /// <summary>
    /// 負責執行遊戲行為（GameAction）
    /// 提供 AddToTop, AddAction 方法，讓其他人加入想要執行的（GameAction）
    /// </summary>
    public class GameActionExecutor : SerializedMonoBehaviour
    {

        /// <summary>
        /// 單例模式
        /// </summary>
        public static GameActionExecutor Instance => GameManager.Instance.GameActionManager;

        public SequenceManager SequenceManager;

        public bool IsExecuting => SequenceManager.IsExecuting;
        
        
        #region 將遊戲行為加入執行緒

        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(GameActionBase action, float delay = 0.1f)
        {
            Instance.SequenceManager.AddSequence(new DelaySequence(new List<GameActionBase>(){action}, delay));
        }
        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddAction(List<GameActionBase> actions, float delay = 0.1f)
        {
            Instance.SequenceManager.AddSequence(new DelaySequence(actions, delay));
        }

        public static void AddActionWithFX(FXSequence fxSequence)
        {
            Instance.SequenceManager.AddSequence(fxSequence);
        }
        

        #endregion

    }

}