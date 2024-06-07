using FoolGame;

namespace FoolGameWpf.entities.bot
{
    public class Node
    {
        public int pointOfMove;
        public Card card;

        public Node(int pointOfMove, Card card)
        {
            this.pointOfMove = pointOfMove;
            this.card = card;
        }
    }
}