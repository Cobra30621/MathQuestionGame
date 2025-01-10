using System;
using System.Collections.Generic;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Card.Data
{
    [CreateAssetMenu(fileName = "角色卡牌圖片", menuName = "角色的卡牌圖片", order = 0)]
    public class CharacterCardSprites : SerializedScriptableObject
    {
        [LabelText("角色的卡牌圖片")]
        [SerializeField] private Dictionary<AllyClassType, CardSprite> _spritesDict;

        public CardSprite GetSprite(AllyClassType type)
        {
            if (_spritesDict.TryGetValue(type, out var image))
                return image;
            else
                Debug.LogError($"找不到 {type} 的卡牌背景");
            return null;
        }
        
    }

    [Serializable]
    public class CardSprite
    {
        [LabelText("角色卡片背景")]
        public Sprite backgroundSprite;
        
        [FormerlySerializedAs("lockCardSprite")] [LabelText("還未獲得的卡片")]
        public Sprite ungainCardSprite;
    }
}