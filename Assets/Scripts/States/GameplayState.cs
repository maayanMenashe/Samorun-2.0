using UnityEngine;

public class GameplayState : IGameState
{
    private GameContext _ctx;

    public void Enter(GameContext ctx)
    {
        _ctx = ctx;

        ctx.Events.OnPlayerDied += HandlePlayerDied;

        ctx.CinematicCamera.enabled = false;
        ctx.CameraController.enabled = true;
        ctx.CameraController.ResetToDefaults();

        ctx.PlayerMotor.enabled = true;

        foreach (var spawner in ctx.Spawners)
        {
            if (spawner != null)
                spawner.enabled = true;
        }

        Time.timeScale = 1f;
    }

    public void Update() { }

    public void Exit()
    {
        _ctx.Events.OnPlayerDied -= HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        _ctx.Flow.TransitionTo(new DeathState());
    }
}
