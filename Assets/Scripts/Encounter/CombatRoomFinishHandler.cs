using Map;
using UnityEngine;
using Utils;

namespace Encounter
{
    [RequireComponent(typeof(SceneChanger))]
    public class CombatRoomFinishHandler : MonoBehaviour
    {
        private SceneChanger _sceneChanger;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }

        public void BackToMap()
        {
            EncounterManager.Instance.OnCompletedCombatRoom();
            if (MapManager.Instance.IsLastRoom())
            {
                if (MapManager.Instance.IsLastMap())
                {
                    _sceneChanger.OpenWinMapScene();
                }
                else
                {
                    _sceneChanger.OpenCompleteMapScene();
                }
            }
            else
            {
                StartCoroutine(_sceneChanger.OpenMapScene());
            }
        }
    }
}