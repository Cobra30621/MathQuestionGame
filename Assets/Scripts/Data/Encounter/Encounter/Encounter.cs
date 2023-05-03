using System;
using System.Collections.Generic;
using NueGames.Data.Characters;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    
    
    
    /// <summary>
    /// 遭遇的基礎類別
    /// </summary>
    [Serializable]
    public abstract class EncounterBase : ScriptableObject
    {
        [SerializeField] private BackgroundTypes targetBackgroundType;
        
        public BackgroundTypes TargetBackgroundType => targetBackgroundType;
        
    }
}