namespace FoolGame
{
    public abstract class MoveState
    { public class MoveEnable : MoveState
        {
            public int firstPosition;
            public int secondPosition;

            public MoveEnable(int firstPosition, int secondPosition)
            {
                this.firstPosition = firstPosition;
                this.secondPosition = secondPosition;
            }
        }
        public class MoveInability : MoveState { }


    }
}