using UnityEngine;

namespace Combat.Card
{
    public static class LayoutCurveUtility
    {
        /// <summary>
        /// 三點計算二次貝茲曲線上一點 (等同 Lerp(Lerp(a,b,t), Lerp(b,c,t), t))。
        /// </summary>
        public static Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return (oneMinusT * oneMinusT * a)
                   + (2f * oneMinusT * t * b)
                   + (t * t * c);
        }

        /// <summary>
        /// 取貝茲曲線切線（Derivative）。
        /// </summary>
        public static Vector3 GetCurveTangent(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return 2f * (1f - t) * (b - a) + 2f * t * (c - b);
        }

        /// <summary>
        /// 回傳與切線垂直的向量，作為卡牌朝向計算時的「上」方向。
        /// </summary>
        public static Vector3 GetCurveNormal(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 tangent = GetCurveTangent(a, b, c, t);
            return Vector3.Cross(tangent, Vector3.forward);
        }
    }
}