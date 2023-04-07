using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Relic;
using NueGames.UI;
using UnityEngine;

public class RelicCanvas : CanvasBase
{
    [SerializeField] private Transform relicIconSpawnRoot;
    private List<RelicIconsBase> _relicIconsBases;
    private RelicIconsBase RelicBasePrefab => RelicManager.relicsData.RelicBasePrefab;
    
    private RelicManager RelicManager => RelicManager.Instance;

    /// <summary>
    /// 取得 Relic 時
    /// </summary>
    /// <param name="relicClip"></param>
    public void OnGainRelic(RelicClip relicClip)
    {
        var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
        clone.SetData(relicClip.Data);
    }
    
    public void ShowCurrentRelicList()
    {
        foreach (var relicClip in RelicManager.CurrentRelicList)
        {
            var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
            clone.SetData(relicClip.Data);
        }
    }
}
