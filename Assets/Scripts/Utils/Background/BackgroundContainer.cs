using System;
using System.Collections.Generic;
using NueGames.Managers;
using UnityEngine;
using NueGames.Combat;

namespace NueGames.Utils.Background
{
    public class BackgroundContainer : MonoBehaviour
    {
        [SerializeField] private List<BackgroundRoot> backgroundRootList;
        public List<BackgroundRoot> BackgroundRootList => backgroundRootList;
        
        private CombatManager CombatManager => CombatManager.Instance;
        
        public void OpenSelectedBackground()
        {
            var encounter = CombatManager.CurrentEncounter;
            foreach (var backgroundRoot in BackgroundRootList)
                backgroundRoot.gameObject.SetActive(encounter.TargetBackgroundType == backgroundRoot.BackgroundType);
        }
    }
}