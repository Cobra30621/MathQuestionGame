using NueGames.Enums;
using UnityEngine;

namespace NueGames.Utils
{
    public class RarityRoot : MonoBehaviour
    {
        [SerializeField] private RarityType rarity;

        public RarityType Rarity => rarity;
    }
}