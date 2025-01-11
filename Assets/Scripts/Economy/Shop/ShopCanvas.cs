using NueGames.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Economy.Shop
{
    /// <summary>
    /// The ShopCanvas class handles the opening and closing of the shop UI canvas.
    /// </summary>
    public class ShopCanvas : CanvasBase
    {
        [Required]
        [SerializeField] private CardShopUI _cardShopUI; // Reference to the CardShopUI component.

        [Required] [SerializeField] private RelicShopUI _relicShopUI;
        
        /// <summary>
        /// Opens the shop canvas and shows the ally class card panel by default.
        /// </summary>
        [Button]
        public override void OpenCanvas()
        {
            base.OpenCanvas();
            
            OpenAllyClassCardPanel();
        }

        /// <summary>
        /// Closes all panels and shows the general card commodities panel in the CardShopUI.
        /// </summary>
        [Button]
        public void OpenGeneralCardPanel()
        {
            CloseAllPanel();
            
            _cardShopUI.gameObject.SetActive(true);
            _cardShopUI.ShowGeneralCardCommodities();
        }

        /// <summary>
        /// Closes all panels and shows the ally class card commodities panel in the CardShopUI.
        /// </summary>
        [Button]
        public void OpenAllyClassCardPanel()
        {
            CloseAllPanel();
            
            _cardShopUI.gameObject.SetActive(true);
            _cardShopUI.ShowAllyClassCardCommodities();
        }

        /// <summary>
        /// Closes all panels and shows the relic commodities panel in the CardShopUI.
        /// Note: This method is currently not implemented.
        /// </summary>
        [Button]
        public void OpenRelicPanel()
        {
            CloseAllPanel();
            
            _relicShopUI.gameObject.SetActive(true);
            _relicShopUI.ShowCommodities();
        }

        /// <summary>
        /// Closes all panels in the CardShopUI.
        /// </summary>
        private void CloseAllPanel()
        {
            _cardShopUI.gameObject.SetActive(false);
            _relicShopUI.gameObject.SetActive(false);
        }
        
    }
}