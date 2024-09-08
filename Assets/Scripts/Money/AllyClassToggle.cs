using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Money
{
    /// <summary>
    /// Handles the toggling of ally class options in the game.
    /// </summary>
    public class AllyClassToggle : MonoBehaviour
    {
        [Required]
        public Toggle toggle;

        [Required]
        public TextMeshProUGUI info;
    }
}