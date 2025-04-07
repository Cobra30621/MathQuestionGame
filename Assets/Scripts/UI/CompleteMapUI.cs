using System;
using Map;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    [RequireComponent(typeof(SceneChanger))]
    public class CompleteMapUI : MonoBehaviour
    {
        private SceneChanger _sceneChanger;

        public TextMeshProUGUI mapName;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
            
            
        }

        private void Start()
        {
            UpdateUI();
        }

        [Button]
        public void UpdateUI()
        {
            var info =  "突破地圖:" + MapManager.Instance.CurrentMap.mapName;
            Debug.Log(info);
            mapName.text = info;
        }

        [Button]
        public void EnterNextMap()
        {
            Debug.Log("EnterNextMap");
            StartCoroutine(_sceneChanger.OpenMapScene());
        }
    }
}