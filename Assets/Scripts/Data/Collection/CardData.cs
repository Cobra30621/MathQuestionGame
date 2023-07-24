using System;
using System.Collections.Generic;
using System.Text;
using NueGames.Action;
using NueGames.Action.MathAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Power;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace NueGames.Data.Collection
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "NueDeck/Collection/Card", order = 0)]
    public class CardData : ScriptableObject
    {
        [Header("Card Profile")] 

        [SerializeField] private string cardName;
        [SerializeField] private int manaCost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;

        [Header("Action Settings")] [SerializeField]
        private ActionTargetType actionTargetType;

        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<ActionData> cardActionDataList;

        [Header("Description")] [SerializeField]
        private List<CardDescriptionData> cardDescriptionDataList;

        [SerializeField] private List<SpecialKeywords> specialKeywordsList;


        #region Cache
        public string CardName => cardName;
        public int ManaCost => manaCost;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        public ActionTargetType ActionTargetType => actionTargetType;
        public List<ActionData> CardActionDataList => cardActionDataList;

        public List<CardDescriptionData> CardDescriptionDataList => cardDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public string MyDescription { get; set; }
        

        public bool ExhaustAfterPlay => exhaustAfterPlay;

        #endregion

        #region Methods

        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in cardDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }

            MyDescription = str.ToString();
        }

        #endregion
        
    }

}