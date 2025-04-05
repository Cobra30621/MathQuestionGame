using System;
using Map;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    [RequireComponent(typeof(SceneChanger))]
    public class CompleteMapUI : SerializedMonoBehaviour
    {
        private SceneChanger _sceneChanger;

        [SerializeField] private TextMeshProUGUI mapName;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
            
            
        }

        private void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            var info =  "突破地圖:" + MapManager.Instance.CurrentMap.mapName;
            mapName.text = info;
        }

        public void EnterNextMap()
        {
            _sceneChanger.OpenMapScene();
        }
    }
}