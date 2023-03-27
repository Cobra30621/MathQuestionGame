using NueGames.Enums;
using UnityEngine;

namespace NueGames.Utils.Background
{
    public class BackgroundRoot : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}