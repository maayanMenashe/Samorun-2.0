using System.Collections;
using UnityEngine;

public class DeathState : IGameState
{
    private GameContext _ctx;
    private Coroutine _deathCoroutine;

    public void Enter(GameContext ctx)
    {
        _ctx = ctx;

        Time.timeScale = ctx.Config.slowMotionScale;
        ctx.PlayerMotor.enabled = false;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyObj in enemies)
        {
            var encounter = enemyObj.GetComponent<EnemyEncounter>();
            if (encounter != null)
                encounter.ForceStopCombat();
        }

        _deathCoroutine = ctx.Flow.StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSecondsRealtime(_ctx.Config.deathAnimWaitSeconds);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Object.Destroy(enemy);
        }

        _ctx.CameraController.ResetToDefaults();

        _ctx.PlayerLife.Respawn();

        Time.timeScale = 1f;

        _ctx.Flow.TransitionTo(new GameplayState());
    }

    public void Update() { }

    public void Exit()
    {
        if (_deathCoroutine != null)
            _ctx.Flow.StopCoroutine(_deathCoroutine);

        Time.timeScale = 1f;
        _ctx.PlayerMotor.enabled = true;
    }
}
