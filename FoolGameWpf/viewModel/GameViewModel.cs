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
        private Card trumpCard;
        private ManagerCards managerCards;

        public ObservableState<ViewState> viewMoveState = new ObservableState<ViewState>();
        public List<Player> Players => players;
        public ObservableCollection<Card> CurrentStack => currentStack;
        public Card TrumpCard => trumpCard;
        public int FirstMove;

        public void StartGame(int playerCounts, string[] playersName, bool isBot = true)
        {
            DeckOfCard.ShuffleDeck();
            SetPlayers(playerCounts, playersName, isBot);
            trumpCard = DeckOfCard.GetCardsFromDeck(1)[0];
            FirstMove = GetFirstMove();
            managerCards = new ManagerCards(FirstMove, trumpCard, players, currentStack);
        }

        public void Move(Card cardMoved = null)
        {
            var moveState = managerCards.Move(cardMoved);

            if (moveState is MoveState.MoveInability)
            {
                viewMoveState.emit(new InabilityMove());
            }

            if (moveState is MoveState.MoveEnable moveEnable)
            {
                viewMoveState.emit(new ChangeCurrentStep(moveEnable.firstPosition, moveEnable.secondPosition));
                if (players[managerCards.currentMove] is BotPlayer)
                    BotMove();
            }
        }

        private void BotMove()
        {
            var card = ((BotPlayer)players[managerCards.currentMove]).GetMoveCard(managerCards.currentMovIsAssault);
            Move(card);
        }

        private int GetFirstMove()
        {
            Value currentMinValue = Value.ACE;
            int i = -1;
            int result = -1;
            foreach (var player in players)
            {
                i++;
                foreach (var card in player.cards)
                {
                    if (card.suit == trumpCard.suit && card.value < currentMinValue)
                    {
                        currentMinValue = card.value;
                        result = i;
                    }
                }
            }

            return result;
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

        public bool MoveIsAssualt()
        {
            return managerCards.currentMovIsAssault;
        }

        public void MoveIfBotRound()
        {
            if (players[FirstMove] is BotPlayer)
            {
                BotMove();
            }
        }
    }
}