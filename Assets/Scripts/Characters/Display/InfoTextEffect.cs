using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Characters.Display
{
    public class InfoTextEffect : MonoBehaviour
    {
        public TextMeshProUGUI infoText; // 指向 TextMeshProUGUI 元素
        public float fadeDuration = 0.5f; // 淡入淡出時間
        public float moveDuration = 1f; // 移動的時間
        public Vector3 moveDirection = new Vector3(0, 100, 0); // 移動的方向

        private void Start()
        {
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 0f); // 初始透明
        }


        public void ShowInfoText(string info)
        {
            StartCoroutine(ShowInfoTextCoroutine(info));
        }
        

        public IEnumerator ShowInfoTextCoroutine(string info)
        {
            // 設置初始狀態
            infoText.text = info;
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 0f); // 初始透明

            // 記錄初始位置
            Vector3 initialPos = infoText.transform.position;
            Vector3 targetPos = initialPos + moveDirection;

            float fadeElapsedTime = 0f;
            float moveElapsedTime = 0f;

            // 同時開始淡入和移動
            while (fadeElapsedTime < fadeDuration || moveElapsedTime < moveDuration)
            {
                if (fadeElapsedTime < fadeDuration)
                {
                    float alpha = Mathf.Lerp(0f, 1f, fadeElapsedTime / fadeDuration);
                    infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, alpha);
                    fadeElapsedTime += Time.deltaTime;
                }

                if (moveElapsedTime < moveDuration)
                {
                    infoText.transform.position = Vector3.Lerp(initialPos, targetPos, moveElapsedTime / moveDuration);
                    moveElapsedTime += Time.deltaTime;
                }

                yield return null;
            }

            // 最終確保文字的完全顯示位置和透明度
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 1f);
            infoText.transform.position = targetPos;

            // 淡出
            yield return FadeText(1f, 0f, fadeDuration);
        }

        private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            Color currentColor = infoText.color;
            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                infoText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            infoText.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);
        }
    }
}