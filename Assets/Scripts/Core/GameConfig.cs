using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Samorun/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    public float runSpeed = 10f;
    public float respawnInvincibilitySeconds = 1.5f;

    [Header("Camera")]
    public float defaultOrthoSize = 13f;
    public float followSpeed = 7f;
    public float combatZoomSize = 5f;

    [Header("Combat")]
    public float slowMotionScale = 0.1f;
    public float deathAnimWaitSeconds = 1f;
    public float qteTimeoutSeconds = 5f;

    [Header("Intro")]
    public float titleFadeDuration = 1f;
    public float titleFadeOutDuration = 0.5f;
    public float pressAnyKeyDelay = 0.5f;
    public bool skipIntro;
    public CameraKeyframe[] introKeyframes;
}
