using System.Collections.Generic;
using System.Linq;
using NueGames.Data.Containers;
using NueGames.Managers;
using NueGames.Relic;
using UnityEngine;

namespace Managers
{
    public class RelicManager : MonoBehaviour
    {
        public static RelicManager Instance { get; private set; }

        private GameManager GameManager => GameManager.Instance;
        private List<RelicClip> currentRelicList => GameManager.PersistentGameplayData.CurrentRelicList;
        [SerializeField] private RelicsData relicsData;
        
        #region SetUp (初始化)
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Initialize();
        }

        void Initialize()
        {
        }

        #endregion
        /// <summary>
        /// 玩家獲得遺物
        /// </summary>
        /// <param name="targetRelic"></param>
        /// <returns></returns>
        public void GainRelic(RelicType targetRelic)
        {
            RelicBase relicBase = RelicGenerator.GetRelic(targetRelic);
            RelicData data = relicsData.RelicList.FirstOrDefault(x => x.RelicType == targetRelic);
            RelicClip relicClip = new RelicClip(relicBase, data);
            
            currentRelicList.Add(relicClip);
        }
        
        #region Debug 用
        /// <summary>
        /// 印出所有的遺物
        /// </summary>
        public void PrintCurrentRelicList()
        {
            foreach (var relicClip in currentRelicList)
            {
                Debug.Log(relicClip);
            }
        }

        #endregion
    }
}