using UnityEngine;

public class CinematicIntroState : IGameState
{
    private GameContext _ctx;

    public void Enter(GameContext ctx)
    {
        _ctx = ctx;

        ctx.CinematicCamera.enabled = true;
        ctx.CameraController.enabled = false;

        ctx.PlayerMotor.enabled = true;

        var configKeyframes = ctx.Config.introKeyframes;
        if (configKeyframes == null || configKeyframes.Length == 0)
        {
            ctx.Flow.TransitionTo(new GameplayState());
            return;
        }

        var keyframes = new CameraKeyframe[configKeyframes.Length];
        System.Array.Copy(configKeyframes, keyframes, configKeyframes.Length);

        int lastIdx = keyframes.Length - 1;
        var lastKf = keyframes[lastIdx];
        lastKf.position = new Vector3(
            ctx.Player.position.x,
            ctx.Player.position.y,
            ctx.MainCamera.transform.position.z);
        keyframes[lastIdx] = lastKf;

        ctx.CinematicCamera.Play(keyframes, () =>
        {
            ctx.Flow.TransitionTo(new GameplayState());
        });
    }

    public void Update() { }

    public void Exit()
    {
        _ctx.CinematicCamera.Stop();
    }
}
