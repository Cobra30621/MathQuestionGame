using System.Collections;
using System.Collections.Generic;
using Managers;
using NueGames.Relic;
using NueGames.UI;
using UnityEngine;
using NueGames.Managers;

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
    public void OnGainRelic(RelicClip relicClip)
    {
        var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
        clone.SetRelicClip(relicClip);
    }
    
    public void ShowCurrentRelicList()
    {
        foreach (var relicClip in RelicManager.CurrentRelicList)
        {
            var clone = Instantiate(RelicBasePrefab, relicIconSpawnRoot);
            clone.SetRelicClip(relicClip);
        }
    }
}
