public abstract class State
{
    public PlayerController player;
    public State(PlayerController player)
    {
        this.player = player;
    }
}