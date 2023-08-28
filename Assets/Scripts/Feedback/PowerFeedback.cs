using MoreMountains.Feedbacks;
using NueGames.Data.Containers;
using NueGames.Power;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feedback
{
    [RequireComponent(typeof(MMF_Player))]
    public class PowerFeedback : MonoBehaviour
    {
        private MMF_Player _mmfPlayer;
        [SerializeField] private PowersData powersData;
        [SerializeField] private Image powerIcon;
        [SerializeField] private TextMeshProUGUI powerName;

        private void Awake()
        {
            _mmfPlayer = GetComponent<MMF_Player>();
        }

        public void Play(PowerName targetPower, bool gainPower)
        {
            var targetData = powersData.GetPowerData(targetPower);
            powerIcon.sprite = targetData.IconSprite;
            powerName.text = gainPower ? $"+ {targetData.GetHeader()}" : $"- {targetData.GetHeader()}";
           
            
            _mmfPlayer.PlayFeedbacks();
        }
    }
}