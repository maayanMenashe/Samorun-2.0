using System;

public class GameEvents
{
    public Action OnCombatStarted;
    public Action OnCombatEnded;
    public Action OnPlayerDied;
    public Action OnPlayerRespawned;
    public Action OnEnemyKilled;
    public Action<IGameState> OnStateChanged;
    public Action OnIntroComplete;
}
