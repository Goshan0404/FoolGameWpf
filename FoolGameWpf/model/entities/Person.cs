using System.Collections.ObjectModel;
using FoolGame;

namespace FoolGameWpf.entities
{
    public class Person: Player
    {
        public Person(ObservableCollection<Card> cards, string name) : base(cards, name)
        {
        }

        public override void Move(Card card)
        {
            cards.Remove(card);
        }
    }
}