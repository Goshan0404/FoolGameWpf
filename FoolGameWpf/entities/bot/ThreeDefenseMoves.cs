using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using FoolGame;
using FoolGameWpf.entities.bot;

namespace FoolGameWpf.entities
{
    public class ThreeDefenseMoves
    {
        private List<Node> nodes = new List<Node>();
        private ObservableCollection<Card> botCards;

        public ThreeDefenseMoves(ObservableCollection<Card> botCards)
        {
            this.botCards = botCards;
        }

        public void FillCards(Card currentCardInStack, List<Card> memoryOfCards)
        {
            nodes.Clear();
            foreach (var card in botCards)
            {
                if (card.HigherThen(currentCardInStack))
                {
                    var countOfPassedCards =
                        memoryOfCards.FindAll(currentCard => currentCard.value == card.value).Count();
                    var pointOfMove = (int)card.value / (4 - countOfPassedCards);
                    nodes.Add(new Node(pointOfMove, card));
                }
            }
        }

        public Card GetBestMove()
        {
            
            
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
    }
}