using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame;

namespace FoolGameWpf.entities.bot
{
    public class BotManagerMoves
    {
        private List<Card> memoryOfCards;
        private ObservableCollection<Card> cardsInStack;
        private ThreeAssaultMove threeAssaultMove;
        private ThreeDefenseMoves threeDefenseMoves; 

        public BotManagerMoves(ObservableCollection<Card> botCards, ObservableCollection<Card> cardsInStack, List<Card> memoryOfCards)
        {
            this.cardsInStack = cardsInStack;
            this.memoryOfCards = memoryOfCards;
            threeDefenseMoves = new ThreeDefenseMoves(botCards);
            threeAssaultMove =  new ThreeAssaultMove(botCards);
        }

        public Card GetMoveCard(bool currentMovIsAssault)
        {
            if (currentMovIsAssault)
            {
                threeAssaultMove.FillCards(memoryOfCards, cardsInStack);
                return threeAssaultMove.GetBestMove(cardsInStack);
            }
            else
            {
                threeDefenseMoves.FillCards(cardsInStack.Last(), memoryOfCards);
                return threeDefenseMoves.GetBestMove();
            }
        }
    }
}