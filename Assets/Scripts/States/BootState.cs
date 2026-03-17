using UnityEngine;

public class BootState : IGameState
{
    private GameFlowController _flow;
    private GameConfig _config;

    public void Initialize(GameFlowController flow, GameConfig config)
    {
        _flow = flow;
        _config = config;

        Application.targetFrameRate = 60;

        var mainCamera = Camera.main;
        if (mainCamera == null) { Debug.LogError("[BootState] Camera.main is null"); return; }

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) { Debug.LogError("[BootState] No GameObject with tag 'Player'"); return; }

        var cinematicCamera = mainCamera.GetComponent<CinematicCamera>();
        if (cinematicCamera == null) { Debug.LogError("[BootState] CinematicCamera not found on main camera"); return; }

        var cameraController = mainCamera.GetComponent<GameplayCameraController>();
        if (cameraController == null) { Debug.LogError("[BootState] GameplayCameraController not found on main camera"); return; }

        var playerMotor = playerObj.GetComponent<CharacterMotor>();
        if (playerMotor == null) { Debug.LogError("[BootState] CharacterMotor not found on player"); return; }

        var playerLife = playerObj.GetComponent<PlayerLife>();
        if (playerLife == null) { Debug.LogError("[BootState] PlayerLife not found on player"); return; }

        var events = new GameEvents();
        var spawners = Object.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        var context = new GameContext(
            mainCamera, cinematicCamera, cameraController,
            playerObj.transform, playerMotor, playerLife, spawners,
            config, events, flow);

        playerMotor.Initialize(events, config.runSpeed);
        playerLife.Initialize(events, config.respawnInvincibilitySeconds);

        var cameraFollowTarget = playerObj.transform.Find("CameraFollowPoint (1)");
        cameraController.Configure(
            cameraFollowTarget != null ? cameraFollowTarget : playerObj.transform,
            config.followSpeed,
            config.defaultOrthoSize);

        foreach (var encounter in Object.FindObjectsByType<EnemyEncounter>(FindObjectsSortMode.None))
        {
            encounter.Initialize(events, config.combatZoomSize, config.defaultOrthoSize, config.qteTimeoutSeconds, config.deathAnimWaitSeconds, config.slowMotionScale);
        }

        foreach (var spawner in spawners)
        {
            spawner.Initialize(events, config);
        }

        playerMotor.enabled = false;
        cameraController.enabled = false;
        cinematicCamera.enabled = false;
        foreach (var spawner in spawners)
        {
            spawner.enabled = false;
        }

        flow.SetContext(context);

        if (config.skipIntro)
            flow.TransitionTo(new GameplayState());
        else
            flow.TransitionTo(new TitleState());
    }

    public void Enter(GameContext ctx) { }
    public void Update() { }
    public void Exit() { }
}
