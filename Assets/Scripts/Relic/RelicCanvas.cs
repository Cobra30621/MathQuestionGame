using System.Collections.Generic;
using Managers;
using Relic.Data;
using UI;
using UnityEngine;

namespace Relic
{
    public class RelicCanvas : CanvasBase
    {
        [SerializeField] private Transform relicIconSpawnRoot;
        private List<RelicIconsBase> _relicIconsBases;
        private RelicIconsBase RelicBasePrefab => RelicManager.relicsData.RelicBasePrefab;
    
        private RelicManager RelicManager => GameManager.Instance.RelicManager;


        private void Awake()
        {
            RelicManager.OnRelicUpdated.AddListener(OnGainRelic);
        }


        /// <summary>
        /// 取得 Relic 時
        /// </summary>
        /// <param name="relicClip"></param>
        public void OnGainRelic(Dictionary<RelicName, RelicBase> relicDict)
        {
            foreach (Transform child in relicIconSpawnRoot)
            {
                Destroy(child.gameObject);
            }
        
            foreach (var pair in relicDict)
            {
                var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
                clone.SetRelicClip(pair.Value);
            }
        }
    }
}
