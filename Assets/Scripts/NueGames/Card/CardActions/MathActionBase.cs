using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using Managers;
using UnityEngine;
namespace NueGames.Card.CardActions

{
    public abstract class MathActionBase: CardActionBase
    {
        protected QuestionManager QuestionManager => QuestionManager.Instance;
        public abstract void OnAnswer();
    }
}