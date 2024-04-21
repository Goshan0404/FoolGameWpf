using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame.entities;
using FoolGame.Properties;

namespace FoolGame
{
    public class Player
    {
        public ObservableCollection<Card> cards;
        public readonly String name;
        public bool isInGame;

        public Player(ObservableCollection<Card> cards, string name)
        {
            this.cards = cards;
            this.name = name;
            // this.cards = new ObservableCollection<Card>(this.cards.OrderBy(card => card.value));
        }

        public void Move(Card card)
        {
            cards.Remove(card);
        }

        // public void AddCard(Card card)
        // {
        //     cards.Add(card);
        //     this.cards = new ObservableCollection<Card>(this.cards.OrderBy(currentCard => card.value));
        // }
    }


}