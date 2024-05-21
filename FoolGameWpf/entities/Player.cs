using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoolGame.entities;
using FoolGame.Properties;

namespace FoolGame
{
    public abstract class Player
    {
        public readonly ObservableCollection<Card> cards;
        public readonly String name;
        public bool isInGame;

        protected Player(ObservableCollection<Card> cards, string name)
        {
            this.cards = cards;
            this.name = name;
        }

        abstract public void Move(Card card);
        
    }


}