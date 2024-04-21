using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FoolGame.entities;
using FoolGame.Properties;

namespace FoolGame
{
    public static class DeckOfCard
    {
        private static Stack<Card> deckOfCard = null;

        private static List<Card> listOfCards = new List<Card>();
        static DeckOfCard()
        {
            SetDeckOfCards();
        }


        private static void SetDeckOfCards()
        {
            foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
                foreach (Value value in (Value[])Enum.GetValues(typeof(Value)))
                    listOfCards.Add(new Card(suit, value, 
                        String.Format("Content/{0}_{1}.gif", suit.ToString(), value.ToString()).ToLower()));

        }

        public static Stack<Card> GetDeckOfCard()
        {
            if (deckOfCard == null)
            {
                deckOfCard = new Stack<Card>(listOfCards);
            }

            return deckOfCard;
        }


        public static void ShuffleDeck()
        {
            int currentCard;
            Card tempCard;
            Random random = new Random();

            for (int i = 0; i < 36; i++)
            {
                currentCard = random.Next(36);
                tempCard = listOfCards[i];
                listOfCards[i] = listOfCards[currentCard];
                listOfCards[currentCard] = tempCard;
            }

            deckOfCard = new Stack<Card>(listOfCards);
        }

        public static ObservableCollection<Card> GetCardsFromDeck(int count)
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            for (int i = 0; i < count; i++)
            {
                if (deckOfCard.Count == 0)
                {
                    return cards;
                }

                cards.Add(deckOfCard.Pop());
            }

            return cards;
        }
    }
}