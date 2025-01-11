using System;
using Question.Enum;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class GradeDropdown : SerializedMonoBehaviour
    {
        private TMP_Dropdown dropdown;

        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            InitializeDropdown();
        }
        
        private void InitializeDropdown()
        {
            // 清空 Dropdown 中的所有選項
            dropdown.ClearOptions();

            // 將 enum 的值轉換為字串陣列
            string[] enumNames = System.Enum.GetNames(typeof(Grade));

            // 將字串陣列轉換為 Dropdown 選項的列表
            var dropdownOptions = new System.Collections.Generic.List<string>(enumNames);

            // 設定 Dropdown 的選項
            dropdown.AddOptions(dropdownOptions);
        }
    }
}