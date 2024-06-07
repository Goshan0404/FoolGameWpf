using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGameWpf;

namespace FoolGame
{
    public class ManagerCards
    {
        private List<Player> players;
        private ObservableCollection<Card> currentStack;

        private Card trumpCard;

        public int currentMove;
        public bool currentMovIsAssault = true;

        public ManagerCards(int firstMove, Card trumpCard, List<Player> players,
            ObservableCollection<Card> currentStack)
        {
            currentMove = firstMove;
            this.players = players;
            this.currentStack = currentStack;
            this.trumpCard = trumpCard;
        }

        public MoveState Move(Card cardMoved = null)
        {
            MoveState result;

            if (cardMoved == null)
            {
                if (currentMovIsAssault)
                {
                    Pass();
                    result = new MoveState.MoveEnable(currentMove, GetNexPlayer());
                    currentMove = GetNexPlayer();
                }
                else
                {
                    TakeCards();
                    result = new MoveState.MoveEnable(currentMove, GetNexPlayer());
                    currentMove = GetNexPlayer();
                }

                currentMovIsAssault = true;
                return result;
            }

            if (!(currentMovIsAssault && CanMoveAssault(cardMoved)) &&
                !(!currentMovIsAssault && CanMoveBack(cardMoved)))
                return new MoveState.MoveInability();


            MakeMove(cardMoved);

            if (currentMovIsAssault)
            {
                result = new MoveState.MoveEnable(currentMove, GetNexPlayer());
                currentMove = GetNexPlayer();
            }
            else
            {
                result = new MoveState.MoveEnable(currentMove, GetPreviousPlayer());
                currentMove = GetPreviousPlayer();
            }

            currentMovIsAssault = !currentMovIsAssault;
            return result;
        }

        private void FillMissingCards()
        {
            foreach (var player in players)
            {
                if (player.cards.Count < 6)
                {
                    foreach (var card in DeckOfCard.GetCardsFromDeck(6 - player.cards.Count))
                    {
                        player.cards.Add(card);
                    }
                }
            }
        }

        private bool CanMoveAssault(Card cardMoved)
        {
            if (currentStack.Count == 0)
                return true;

            foreach (var card in currentStack)
            {
                if (card.value == cardMoved.value)
                    return true;
            }

            return false;
        }

        private bool CanMoveBack(Card card)
        {
            if ((currentStack.Last().suit == card.suit && currentStack.Last().value < card.value)
                || (currentStack.Last().suit != trumpCard.suit && trumpCard.suit == card.suit))
            {
                return true;
            }

            return false;
        }

        private void MakeMove(Card card)
        {
            players[currentMove].Move(card);
            currentStack.Add(card);
        }

        private int GetNexPlayer()
        {
            if (currentMove + 1 > players.Count - 1)
                return 0;
            return currentMove + 1;
        }

        private int GetPreviousPlayer()
        {
            if (currentMove == 0)
                return players.Count - 1;
            return currentMove - 1;
        }

        private void Pass()
        {
            currentStack.Clear();
            FillMissingCards();
        }

        private void TakeCards()
        {
            foreach (var card in currentStack)
            {
                players[currentMove].cards.Add(card);
            }

            currentStack.Clear();
            FillMissingCards();
        }
    }
}