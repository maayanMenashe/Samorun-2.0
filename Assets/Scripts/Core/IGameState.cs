public interface IGameState
{
    void Enter(GameContext ctx);
    void Update();
    void Exit();
}
