using System;
using System.Collections.Generic;
using NueGames.Data.Characters;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    
    
    
    /// <summary>
    /// 遭遇的基礎類別
    /// </summary>
    [Serializable]
    public abstract class EncounterBase : SerializedScriptableObject ,ISerializeReferenceByAssetGuid
    {
        [SerializeField] private BackgroundTypes targetBackgroundType;
        
        public BackgroundTypes TargetBackgroundType => targetBackgroundType;
        
    }
}