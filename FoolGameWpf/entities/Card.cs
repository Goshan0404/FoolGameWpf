using FoolGame.entities;
using FoolGame.Properties;

namespace FoolGame
{
    public class Card
    {
        public readonly Suit suit;
        public readonly Value value;
        public string imageName { get; set; }

        public Card(Suit suit, Value value, string imageName)
        {
            this.suit = suit;
            this.value = value;
            this.imageName = imageName;
        }

        public bool HigherThen(Card card)
        {
            return value > card.value && suit == card.suit;
        }

        public override string ToString()
        {
            return value + " of " + suit;
        }
    }
}