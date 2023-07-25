using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardAction;
using NueGames.Action;
using NueGames.Action.MathAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Power;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace NueGames.Data.Collection
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "NueDeck/Collection/Card", order = 0)]
    public class CardData : SerializedScriptableObject
    {
        [Header("卡片行為")]
        [SerializeField] private CardActionBase cardAction;
        
        [Title("數值參數")]
        [SerializeField]private ActionTargetType actionTargetType;
        [SerializeField] private int manaCost;
        [SerializeField] private bool exhaustAfterPlay;
        
        [SerializeField] private  FxName FxName;
        [SerializeField] private FxSpawnPosition FxSpawnPosition;
        
        [Title("卡牌顯示")]
        [SerializeField] private string cardName;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;
        [TextArea(4, 10)] [SerializeField] private string description;

        [SerializeField] private List<SpecialKeywords> specialKeywordsList;


        #region Cache
        public string CardName => cardName;
        public int ManaCost => manaCost;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        public ActionTargetType ActionTargetType => actionTargetType;
        [TypeFilter("GetFilteredTypeList")]
        public CardActionBase CardAction => cardAction;

        public string Description => description;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public string MyDescription { get; set; }
        

        public bool ExhaustAfterPlay => exhaustAfterPlay;

        #endregion

        #region Methods
        
        
        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(CardActionBase).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
                .Where(x => typeof(CardActionBase).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
    
    
            return q;
        }

        public void UpdateDescription()
        {
            
            MyDescription = description;
        }

        #endregion
        
    }

}