using System;
using System.Collections.Generic;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Special Keyword", menuName = "NueDeck/Containers/Special Keyword Data", order = 0)]
    public class SpecialKeywordData : ScriptableObject
    {
        [SerializeField] private List<SpecialKeywordBase> specialKeywordBaseList;
        public List<SpecialKeywordBase> SpecialKeywordBaseList => specialKeywordBaseList;
        
        
    }

    [Serializable]
    public class SpecialKeywordBase
    {
        [SerializeField] private SpecialKeywords specialKeyword;
        [SerializeField] private string titleText;
        [SerializeField][TextArea] private string contentText;

        public SpecialKeywords SpecialKeyword => specialKeyword;
        
        
        public string GetHeader(string overrideKeywordHeader = "")
        {
            if(titleText != "")
                return titleText;
            return string.IsNullOrEmpty(overrideKeywordHeader) ? specialKeyword.ToString() : overrideKeywordHeader;
        }

        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }
    }
}