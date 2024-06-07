namespace FoolGame
{
    public abstract class ViewState { }

    public class ChangeCurrentStep : ViewState
    {

        public int FirstPosition;
        public int SecondPosition;
        public ChangeCurrentStep(int firstPosition, int secondPosition)
        {
            FirstPosition = firstPosition;
            SecondPosition = secondPosition;
        }
    }
    public class InabilityMove: ViewState{}
    

    
}