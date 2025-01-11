using System.Collections.Generic;
using UnityEngine;

namespace Feedback
{
    /// <summary>
    /// 
    /// </summary>
    public class RadarTest : MonoBehaviour
    {
        public UIPolygon uiPolygon;
        List<float> datas = new List<float>();

        void Start()
        {
            // N
            datas.Add(0.92f);
            // S
            datas.Add(0.31f);
            // R
            datas.Add(0.36f);
            // D
            datas.Add(0.28f);
            uiPolygon.DrawPolygon(datas);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                for(int i=0,cnt = datas.Count;i<cnt;++i)
                {
                    datas[i] = Random.Range(0f, 1f);
                }
            
                uiPolygon.DrawPolygon(datas);
            }
        }

    }
}
