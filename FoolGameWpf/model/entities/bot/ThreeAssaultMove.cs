using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame;

namespace FoolGameWpf.entities.bot
{
    public class ThreeAssaultMove
    {
        private List<Node> nodes = new List<Node>();
        private ObservableCollection<Card> botCards;
        private ObservableCollection<Card> cardsInStack;


        public ThreeAssaultMove(ObservableCollection<Card> cards)
        {
            botCards = cards;
        }

        public void FillCards(List<Card> memoryOfCards, ObservableCollection<Card> cardsInStack)
        {
            nodes.Clear();
            foreach (var card in botCards)
            {
                if (cardsInStack.Count(cardInStack => cardInStack.value == card.value) > 0)
                {


                    var countOfPassedCards =
                        memoryOfCards.FindAll(currentCard => currentCard.value == card.value).Count();
                    var pointOfMove = (int)card.value / (4 - countOfPassedCards);
                    nodes.Add(new Node(pointOfMove, card));
                }
            }

            this.cardsInStack = cardsInStack;
        }

        public Card GetBestMove(ObservableCollection<Card> cardsInStack)
        {
            
            if (cardsInStack.Count == 0)
            {
                return GetMinCard();
            }
            
            
            int bestPoint = int.MaxValue;
            Card card = null;
            foreach (var node in nodes)
            {
                if (bestPoint > node.pointOfMove)
                {
                    bestPoint = node.pointOfMove;
                    card = node.card;
                }

                bestPoint = Math.Min(bestPoint, node.pointOfMove);
            }

            return card;
        }

        private Card GetMinCard()
        {
            var minValue = int.MaxValue;
            Card card = null;

            foreach (var botCard in botCards)
            {
                if (minValue > (int) botCard.value)
                {
                    minValue = (int) botCard.value;
                    card = botCard;
                }
            }

            return card;
        }
    }
}