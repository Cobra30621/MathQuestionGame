using System.Collections.Generic;
using Managers;
using NueGames.Relic;
using UnityEngine;
using UnityEngine.Events;

namespace Tool
{
    /// <summary>
    /// 給遊戲開發者測試用的創造器
    /// </summary>
    public class DevelopCreator : MonoBehaviour
    {
        /// <summary>
        /// 遊戲開始時執行
        /// </summary>
        public bool PlayOnStart;
        /// <summary>
        /// 測試的事件
        /// </summary>
        public UnityEvent TestEvent;
        /// <summary>
        /// 測試創建的遺物
        /// </summary>
        public List<RelicType> createRelicList;
        
        void Start()
        {
            if (PlayOnStart)
            {
                PlayTest();
            }
        }
        
        [ContextMenu("使用測試方法")]
        public void PlayTest()
        {
            TestEvent.Invoke();
            GainRelic();
        }
        
        /// <summary>
        /// 取得遺物
        /// </summary>
        private void GainRelic()
        {
            foreach (var relicType in createRelicList)
            {
                RelicManager.Instance.GainRelic(relicType);
                RelicManager.Instance.PrintCurrentRelicList();
            }
        }
    }
}