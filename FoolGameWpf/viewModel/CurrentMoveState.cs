namespace FoolGame
{
    public abstract class CurrentMoveState { }
    
    public class ChangeMove : CurrentMoveState{}
    public class InabilityMove: CurrentMoveState{}
    
    public class AssaultMove: CurrentMoveState{}
    public class DefenseMove: CurrentMoveState{}

    
}