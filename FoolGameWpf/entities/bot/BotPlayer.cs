using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame;
using FoolGameWpf.entities.bot;

namespace FoolGameWpf
{
    public class BotPlayer : Player
    {
        private List<Card> memoryOfCards = new List<Card>();
        private ManagerMoves managerMoves;


        public BotPlayer(ObservableCollection<Card> cards, ObservableCollection<Card> currentStackCards) : base(cards,
            "Bot")
        {
            managerMoves = new ManagerMoves(cards, currentStackCards,
                memoryOfCards);
        }

        public Card GetMoveCard(bool currentMoveIsAssault)
        {
            return managerMoves.GetMoveCard(currentMoveIsAssault);
        }

        public override void Move(Card card)
        {
            cards.Remove(card);
        }
    }
}