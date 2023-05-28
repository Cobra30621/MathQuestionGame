using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Enums;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 特效(FX)的GameObject資料
    /// </summary>
    [CreateAssetMenu(fileName = "FX Data", menuName = "NueDeck/Containers/FX Data", order = 2)]
    public class FXData : ScriptableObject
    {
        [SerializeField] private List<FxBundle> fxList;

        private Dictionary<FxType, GameObject> fxDict;

        public void Init()
        {
            fxDict = new Dictionary<FxType, GameObject>();
            for (int i = 0; i < Enum.GetValues(typeof(FxType)).Length; i++)
                fxDict.Add((FxType)i, fxList.FirstOrDefault(x=>x.FxType == (FxType)i)?.FxPrefab);
        }


        public GameObject GetFX(FxType fxType)
        {
            if (fxDict.ContainsKey(fxType))
            {
                return fxDict[fxType];
            }
            else
            {
                Debug.LogError($"fxDict 沒有 {fxType} 的特效，請去設定");
                return null;
            }
        }
        
    }
    
    
    [Serializable]
    public class FxBundle
    {
        [SerializeField] private FxType fxType;
        [SerializeField] private GameObject fxPrefab;
        public FxType FxType => fxType;
        public GameObject FxPrefab => fxPrefab;
    }
}