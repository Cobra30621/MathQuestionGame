using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 一輪遊戲的地圖資料
    /// </summary>
    [CreateAssetMenu(fileName = "MapData", menuName = "NueDeck/Map/MapData")]
    public class MapData : ScriptableObject
    {
        public List<MapConfig> mapConfigs;

    }
}