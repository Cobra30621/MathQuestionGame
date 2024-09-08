// File: /Users/cobra/Desktop/Unity/Develop/MathQuestionGame/Assets/Scripts/Relic/RelicLevelHandler.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NueGames.Data.Containers;
using NueGames.Relic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Handles the leveling and saving of relics in the game.
/// </summary>
public class RelicLevelHandler : SerializedMonoBehaviour, IPermanentDataPersistence
{
    [SerializeField] public Dictionary<RelicName, RelicLevelInfo> relicLevelInfos;

    [SerializeField]
    public RelicsData relicsData;

    public void InitRelicLevels()
    {
        relicLevelInfos = new Dictionary<RelicName, RelicLevelInfo>();

        foreach (var pair in relicsData.RelicDict)
        {
            var relicId = pair.Key;
            relicLevelInfos[relicId] = new RelicLevelInfo { Level = 0, HasGained = false };
        }
    }

    public int UpgradeRelic(RelicName name)
    {
        bool contain = relicLevelInfos.TryGetValue(name, out var relicSaveInfo);

        Debug.Log($"Upgrade relic {name} to {relicSaveInfo.Level + 1}");
        if (contain)
        {
            relicSaveInfo.Level++;
        }

        SaveManager.Instance.SavePermanentGame();

        return relicSaveInfo.Level;
    }

    public void OnGainRelic(RelicName relicName)
    {
        relicLevelInfos[relicName].HasGained = true;
    }


    public RelicLevelInfo GetRelicSaveInfo(RelicName relicName)
    {
        return relicLevelInfos.TryGetValue(relicName, out var relicSaveInfo) ? 
            relicSaveInfo : new RelicLevelInfo();
    }
    

    public void LoadData(PermanentGameData data)
    {
        InitRelicLevels();

        var loadRelicInfo = data.relicInfo;
        if (loadRelicInfo != null)
        {
            // SaveDeck content takes precedence
            foreach (var relicId in relicLevelInfos.Keys.ToList())
            {
                if (loadRelicInfo.ContainsKey(relicId))
                {
                    relicLevelInfos[relicId] = loadRelicInfo[relicId];
                }
            }
        }
    }

    public void SaveData(PermanentGameData data)
    {
        data.relicInfo = relicLevelInfos;
    }
}

[Serializable]
public class RelicLevelInfo
{
    public bool IsMaxLevel()
    {
        return Level > 0;
    }
    
    public int Level;
    public bool HasGained;
}