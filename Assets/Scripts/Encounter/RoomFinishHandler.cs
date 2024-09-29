using System;
using Map;
using NueGames.Managers;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Encounter
{
    [RequireComponent(typeof(SceneChanger))]
    public class RoomFinishHandler : SerializedMonoBehaviour
    {
        private SceneChanger _sceneChanger;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }

        public void BackToMap()
        {
            EncounterManager.Instance.OnRoomCompleted();
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
                _sceneChanger.OpenMapScene();
            }
        }
    }
}