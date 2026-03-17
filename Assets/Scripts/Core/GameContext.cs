using UnityEngine;

public class GameContext
{
    public Camera MainCamera { get; }
    public CinematicCamera CinematicCamera { get; }
    public GameplayCameraController CameraController { get; }
    public Transform Player { get; }
    public CharacterMotor PlayerMotor { get; }
    public PlayerLife PlayerLife { get; }
    public EnemySpawner[] Spawners { get; }
    public GameConfig Config { get; }
    public GameEvents Events { get; }
    public GameFlowController Flow { get; }

    public GameContext(
        Camera mainCamera,
        CinematicCamera cinematicCamera,
        GameplayCameraController cameraController,
        Transform player,
        CharacterMotor playerMotor,
        PlayerLife playerLife,
        EnemySpawner[] spawners,
        GameConfig config,
        GameEvents events,
        GameFlowController flow)
    {
        MainCamera = mainCamera;
        CinematicCamera = cinematicCamera;
        CameraController = cameraController;
        Player = player;
        PlayerMotor = playerMotor;
        PlayerLife = playerLife;
        Spawners = spawners;
        Config = config;
        Events = events;
        Flow = flow;
    }
}
