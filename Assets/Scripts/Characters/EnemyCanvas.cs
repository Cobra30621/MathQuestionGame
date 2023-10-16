using NueGames.Data.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;

namespace NueGames.Characters
{
    public class EnemyCanvas : CharacterCanvas
    {
        [Header("Enemy Canvas Settings")]
        [SerializeField] private Image intentImage;
        [SerializeField] private TextMeshProUGUI nextActionValueText;
        public EnemyIntentionData IntentionData;
        public GameObject IntentionGO => intentionGO;
        public Image IntentImage => intentImage;
        public TextMeshProUGUI NextActionValueText => nextActionValueText;

        public override void ShowTooltipInfo()
        {
            var tooltipManager = TooltipManager.Instance;
            ShowTooltipInfo(tooltipManager,IntentionData.Content,IntentionData.Header,descriptionRoot);
            ShowPowerTooltipInfo();
        }
    }
}