using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Utils.Background
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