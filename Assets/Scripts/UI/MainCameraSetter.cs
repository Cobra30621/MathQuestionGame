using UnityEngine;

namespace NueGames.UI
{
    public class MainCameraSetter : MonoBehaviour
    {
        private Canvas _canvas;
        
        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("CanvasBase 必需有 Canvas 作為子物件！");
            }
            BindCamera();
        }
        
        private void BindCamera()
        {
            // 尋找場景中的主攝影機
            Camera mainCamera = Camera.main;
            Debug.LogWarning($"{gameObject.name} 找到主攝影機 {mainCamera}");
            if (mainCamera != null && _canvas != null)
            {
                _canvas.worldCamera = mainCamera;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} 未找到主攝影機，無法綁定到 Canvas！");
            }
        }
    }
}