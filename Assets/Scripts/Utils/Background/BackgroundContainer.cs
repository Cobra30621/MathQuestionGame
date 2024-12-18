﻿using System;
using System.Collections.Generic;
using Combat;
using NueGames.Managers;
using UnityEngine;
using NueGames.Combat;

namespace NueGames.Utils.Background
{
    public class BackgroundContainer : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;
        
        
        public void OpenSelectedBackground()
        {
            var encounter = CombatManager.Instance.currentEncounter;
            foreach (var backgroundRoot in BackgroundRootList)
                backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
        }
    }
}