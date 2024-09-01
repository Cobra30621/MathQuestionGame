using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.Relic;
using NueGames.UI;
using UnityEngine;
using NueGames.Managers;
using Relic;

public class RelicCanvas : CanvasBase
{
    [SerializeField] private Transform relicIconSpawnRoot;
    private List<RelicIconsBase> _relicIconsBases;
    private RelicIconsBase RelicBasePrefab => RelicManager.relicsData.RelicBasePrefab;
    
    private RelicManager RelicManager => GameManager.Instance.RelicManager;




    /// <summary>
    /// 取得 Relic 時
    /// </summary>
    /// <param name="relicClip"></param>
    public void OnGainRelic(Dictionary<RelicName, RelicBase> relicDict)
    {
        foreach (Transform child in relicIconSpawnRoot)
        {
            Destroy(child);
        }
        
        Debug.Log(relicDict.Keys);
        
        foreach (var pair in relicDict)
        {
            var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
            clone.SetRelicClip(pair.Value);
        }
    }
}
