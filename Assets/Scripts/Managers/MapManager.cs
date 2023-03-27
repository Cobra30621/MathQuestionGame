using System.Collections.Generic;
using NueGames.Enums;
using NueGames.UI;
using UnityEngine;

namespace NueGames.Managers
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private List<EncounterButton> encounterButtonList;

        public List<EncounterButton> EncounterButtonList => encounterButtonList;
        
        private GameManager GameManager => GameManager.Instance;
        
        private void Start()
        {
            PrepareEncounters();
        }
        
        private void PrepareEncounters()
        {
            for (int i = 0; i < EncounterButtonList.Count; i++)
            {
                var btn = EncounterButtonList[i];
                if (GameManager.PersistentGameplayData.CurrentEncounterId == i)
                    btn.SetStatus(EncounterButtonStatus.Active);
                else if (GameManager.PersistentGameplayData.CurrentEncounterId > i)
                    btn.SetStatus(EncounterButtonStatus.Completed);
                else
                    btn.SetStatus(EncounterButtonStatus.Passive);
            }
        }
    }
}
