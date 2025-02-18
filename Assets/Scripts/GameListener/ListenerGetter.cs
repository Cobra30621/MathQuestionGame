using System.Collections.Generic;
using Combat;
using Managers;
using Relic;
using UnityEngine;

namespace GameListener
{
    /// <summary>
    /// 取得指定的 GameEventListener(遊戲事件監聽器)
    /// 包含玩家持有的能力、敵人們持有的能力、遺物
    /// </summary>
    public static class ListenerGetter
    {
        private static CharacterHandler _characterHandler;
        private static RelicManager _relicManager;



        public static List<GameEventListener> GetAllEventsListeners()
        {
            var listeners = new List<GameEventListener>();
            listeners.AddRange(GetRelicEventsListeners());
            listeners.AddRange(GetMainAllyPowerListeners());
            listeners.AddRange(GetEnemiesPowerListeners());
            return listeners;
        }
        
        
        #region 取得事件監聽者 (能力、遺物)

        public static List<GameEventListener> GetRelicEventsListeners()
        {
            var listeners = new List<GameEventListener>();
            if (GetRelicManager())
            {
                listeners.AddRange(_relicManager.CurrentRelicDict.Values);
            }

            return listeners;
        }
        
        private static List<GameEventListener> GetEnemiesPowerListeners()
        {
            var listeners = new List<GameEventListener>();
            if (GetCharacterHandler())
            {
                foreach (var enemy in _characterHandler.Enemies) 
                {
                    listeners.AddRange(enemy.GetPowerListeners());   
                }
            }
            
            return listeners;
        }
        
        private static List<GameEventListener> GetMainAllyPowerListeners()
        {
            var listeners = new List<GameEventListener>();
            if (GetCharacterHandler())
            {
                listeners = _characterHandler.MainAlly.GetPowerListeners();
            }
            return listeners;
        }


        private static bool GetRelicManager()
        {
            _relicManager = GameManager.Instance.RelicManager;
            
            if (_relicManager == null)
            {
                Debug.LogError("RelicManager is null");
            }
            
            return _relicManager!= null;
        }
        
        private static bool GetCharacterHandler()
        {
            _characterHandler = CombatManager.Instance.characterHandler;
            
            if (_characterHandler == null)
            {
                Debug.LogError("CharacterHandler is null");
            }
            
            return _characterHandler!= null;
        }

        #endregion
    }
}