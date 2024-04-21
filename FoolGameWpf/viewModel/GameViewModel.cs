using FoolGame.entities;
using FoolGame.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame.Utils;


namespace FoolGame
{
    internal class GameViewModel
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
        public int CurrentMove => currentMove;
        public Card TrumpCard => trumpCard;

        public void StartGame(int playerCounts, String[] playersName)
        {
            if (playerCounts > 6)
                throw new Exception();

            DeckOfCard.ShuffleDeck();
            SetPlayers(playerCounts, playersName);
            trumpCard = DeckOfCard.GetCardsFromDeck(1)[0];
            currentMove = SetFirstMove();
            currentMovIsAssault = true;
            currentMoveStates.emit(new AssaultMove());
        }


        public void Move(Card cardMoved)
        {
            if (currentMovIsAssault)
            {
                MoveAssault(cardMoved);
            }
            else
            {
                MoveBack(cardMoved);
            }
        }

        private void MoveAssault(Card cardMoved)
        {
            if (currentStack.Count == 0)
            {
                Move(currentMove, cardMoved);
                SetNextMove();
                currentMovIsAssault = !currentMovIsAssault;
                currentMoveStates.emit(new DefenseMove());
                return;
            }

            foreach (var card in currentStack)
            {
                if (card.value == cardMoved.value)
                {
                    Move(currentMove, cardMoved);
                    SetNextPlayersMove();
                    currentMovIsAssault = !currentMovIsAssault;
                    currentMoveStates.emit(new DefenseMove());
                    return;
                }
            }

            currentMoveStates.emit(new InabilityMove());
        }

        public void MoveBack(Card card)
        {
            if ((currentStack.Last().suit == card.suit && currentStack.Last().value < card.value)
                || (currentStack.Last().suit != trumpCard.suit && trumpCard.suit == card.suit))
            {
                Move(currentMove, card);
                SetNextPlayersMove();
                currentMovIsAssault = !currentMovIsAssault;
                currentMoveStates.emit(new AssaultMove());
                return;
            }

            currentMoveStates.emit(new InabilityMove());
        }

        private void Move(int currentPlayer, Card card)
        {
            players[currentPlayer].Move(card);
            currentStack.Add(card);
        }

        private void SetNextPlayersMove()
        {
            if (currentMove + 1 > players.Count - 1)
            {
                currentMoveStates.emit(new FirstPlayerMove());
                currentMove = 0;
            }
            else
            {
                currentMoveStates.emit(new SecondPlayerMove());
                currentMove += +1;
            }
        }

        private void SetNextMove(int step = 1)
        {
            FillMissingCards();
            for (int i = 0; i < step; i++)
            {
                SetNextPlayersMove();
            }
        }

        public void Pass()
        {
            currentStack.Clear();
            SetNextMove();
        }

        public void TakeCards()
        {
            foreach (var card in currentStack)
            {
                players[currentMove].cards.Add(card);
            }

            currentStack.Clear();
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

        private int SetFirstMove()
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

            if (firstMove == 0)
                currentMoveStates.emit(new FirstPlayerMove());
            else
                currentMoveStates.emit(new SecondPlayerMove());

            return firstMove;
        }


        private void SetPlayers(int playerCounts, string[] names)
        {
            for (int i = 0; i < playerCounts; i++)
            {
                players.Add(new Player(DeckOfCard.GetCardsFromDeck(6), names[i]));
            }
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