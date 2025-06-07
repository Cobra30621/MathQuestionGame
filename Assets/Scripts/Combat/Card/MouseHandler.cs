using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Card
{
    [Serializable]
    public class MouseHandler
    {
        private readonly Camera _camera;
        private readonly Plane _plane;
        private readonly bool _cardTilt;

        private Vector2 _prevMousePos;
        private Vector2 _force;
        private Vector2 _heldCardTilt;

        /// <summary>滑鼠在畫面 (螢幕) 的座標 (範圍已 clamp 在 [0,Screen.Width] x [0,Screen.Height])。</summary>
        [ShowInInspector]
        public Vector2 MousePos { get; private set; }

        /// <summary>滑鼠在畫面上的位移向量 (簡化後用於卡牌傾斜計算)。</summary>
        [ShowInInspector]
        public Vector2 MousePosDelta { get; private set; }

        
        /// <summary>滑鼠在世界座標系下的座標 (對應 XY 平面)。</summary>
        [ShowInInspector]
        public Vector3 MouseWorldPos { get; private set; }

        /// <summary>滑鼠左鍵是否正在按下 (參考 Input.GetMouseButton(0))。</summary>
        public bool MouseButton => Input.GetMouseButton(0);

        /// <summary>計算後的卡牌傾斜向量 (由 HandController 使用，用在卡牌傾斜效果)。</summary>
        public Vector2 HeldCardTilt => _heldCardTilt;

        public MouseHandler(Camera camera, Plane plane, bool cardTilt)
        {
            _camera = camera;
            _plane = plane;
            _cardTilt = cardTilt;

            _prevMousePos = Input.mousePosition;
            _force = Vector2.zero;
            _heldCardTilt = Vector2.zero;
        }

        /// <summary>
        /// 每一幀呼叫一次，內部會：
        /// 1. 計算滑鼠座標 (clamp 後)；
        /// 2. 若開啟卡牌傾斜效果，則計算滑鼠位移與卡牌傾斜向量；
        /// 3. 以射線將滑鼠螢幕座標轉成世界座標 (XY 平面)。
        /// </summary>
        public void Update()
        {
            // 1. 取得並 clamp 滑鼠在螢幕上的座標
            Vector2 rawMousePos = Input.mousePosition;
            rawMousePos.x = Mathf.Clamp(rawMousePos.x, 0, Screen.width);
            rawMousePos.y = Mathf.Clamp(rawMousePos.y, 0, Screen.height);
            MousePos = rawMousePos;

            // 2. 若啟用卡牌傾斜，計算傾斜向量
            if (_cardTilt)
            {
                MousePosDelta = (rawMousePos - _prevMousePos) * new Vector2(1600f / Screen.width, 900f / Screen.height) * Time.deltaTime;
                _prevMousePos = rawMousePos;

                float tiltStrength = 3f;
                float tiltDrag = 3f;
                float tiltSpeed = 50f;

                _force += (MousePosDelta * tiltStrength - _heldCardTilt) * Time.deltaTime;
                _force *= 1 - tiltDrag * Time.deltaTime;
                _heldCardTilt += _force * (Time.deltaTime * tiltSpeed);

                // 若為 Debug 模式，可畫出輔助射線
                if (Debug.isDebugBuild)
                {
                    Debug.DrawRay(MouseWorldPos, MousePosDelta, Color.red);
                    Debug.DrawRay(MouseWorldPos, _heldCardTilt, Color.cyan);
                }
            }

            // 3. 射線計算滑鼠在世界座標系(以 XY 平面為底) 的落點
            Ray ray = _camera.ScreenPointToRay(rawMousePos);
            if (_plane.Raycast(ray, out float enter))
            {
                MouseWorldPos = ray.GetPoint(enter);
            }
        }
    }
}
