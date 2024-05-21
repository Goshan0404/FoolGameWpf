using FoolGame.entities;
using FoolGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame.Utils;
using FoolGameWpf;
using FoolGameWpf.entities;


namespace FoolGame
{
    public class GameViewModel
    {
        private List<Player> players = new List<Player>();
        private ObservableCollection<Card> currentStack = new ObservableCollection<Card>();

        private bool gameIsOver;
        private int currentMove;
        private Card trumpCard;
        private bool currentMovIsAssault;

        public ObservableState<CurrentMoveState> currentMoveStates = new ObservableState<CurrentMoveState>();

        public List<Player> Players => players;
        public ObservableCollection<Card> CurrentStack => currentStack;
        public bool GameIsOver => GameIsFinished();
        public bool CurrentMoveIsAssault => currentMovIsAssault;
        public int CurrentMove => currentMove;

        public Card TrumpCard => trumpCard;

        public void StartGame(int playerCounts, string[] playersName, bool isBot = true)
        {
            DeckOfCard.ShuffleDeck();
            SetPlayers(playerCounts, playersName, isBot);
            trumpCard = DeckOfCard.GetCardsFromDeck(1)[0];
            SetFirstMove();
            currentMovIsAssault = true;
            currentMoveStates.emit(new AssaultMove());

            if (players[currentMove] is BotPlayer)
            {
                BotMove();
            }
        }

        public void Move(Card cardMoved = null)
        {
            if (cardMoved == null)
            {
                if (currentMovIsAssault)
                {
                    Pass();
                    SetNextMove();
                }
                else
                {
                    TakeCards();
                    SetNextMove(2);
                    currentMovIsAssault = !currentMovIsAssault;
                }
            }

            else if (!(currentMovIsAssault && CanMoveAssault(cardMoved)) &&
                !(!currentMovIsAssault && CanMoveBack(cardMoved)))
            {
                currentMoveStates.emit(new InabilityMove());
                return;
            }

            MakeMove(cardMoved);

            if (cardMoved != null)
            {
                SetNextMove();
                currentMovIsAssault = !currentMovIsAssault;
            }

            if (players[currentMove] is BotPlayer)
            {
                BotMove();
            }
        }

        private void ChangeMove()
        {
            if (currentMovIsAssault)
                currentMoveStates.emit(new AssaultMove());
            else
                currentMoveStates.emit(new DefenseMove());
            currentMoveStates.emit(new ChangeMove());
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

        private void SetNextMove(int step = 1)
        {
            for (int i = 0; i < step; i++)
            {
                if (currentMove + 1 > players.Count - 1)
                    currentMove = 0;
                else
                    currentMove += +1;
            }

            ChangeMove();
        }

        private void BotMove()
        {
            var card = ((BotPlayer)players[currentMove]).GetMoveCard(currentMovIsAssault);
            Move(card);
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
            SetNextMove(2);
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

        private void SetFirstMove()
        {
            Value currentMinValue = Value.ACE;
            int i = -1;
            int firstMove = -1;
            foreach (var player in players)
            {
                i++;
                foreach (var card in player.cards)
                {
                    if (card.suit == trumpCard.suit && card.value < currentMinValue)
                    {
                        currentMinValue = card.value;
                        firstMove = i;
                    }
                }
            }

            currentMove = firstMove;
            if (firstMove != 0)
                currentMoveStates.emit(new ChangeMove());
        }


        private void SetPlayers(int playerCounts, IReadOnlyList<string> names, bool isBot)
        {
            for (int i = 0; i < playerCounts - 1; i++)
            {
                players.Add(new Person(DeckOfCard.GetCardsFromDeck(6), names[i]));
            }

            if (isBot)
                players.Add(new BotPlayer(DeckOfCard.GetCardsFromDeck(6), currentStack));
            else
                players.Add(new Person(DeckOfCard.GetCardsFromDeck(6), names[playerCounts - 1]));
        }

        private bool GameIsFinished()
        {
            if (DeckOfCard.GetDeckOfCard().Count == 0)
            {
                foreach (var player in players)
                {
                    if (player.cards.Count == 0)
                        return true;
                }
            }

            return false;
        }
    }
}