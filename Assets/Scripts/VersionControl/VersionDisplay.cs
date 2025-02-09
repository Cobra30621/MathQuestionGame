using System;
using Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace VersionControl
{
    /// <summary>
    /// 顯示目前版本
    /// </summary>
    public class VersionDisplay : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI info;

        private void Awake()
        {
            info.text = "Version : " + GameManager.Instance.SystemVersion().GetVersionNum();
        }
    }
}