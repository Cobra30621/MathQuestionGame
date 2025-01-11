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
            
            UpdateUI();
        }

        public void UpdateUI()
        {
            mapName.text = "突破地圖:" + MapManager.Instance.CurrentMap.mapName;
        }

        public void EnterNextMap()
        {
            _sceneChanger.OpenMapScene();
        }
    }
}