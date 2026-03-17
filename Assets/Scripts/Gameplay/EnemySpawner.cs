using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Vector3 _spawnOffset = Vector3.left;

    private bool _hasSpawned;
    private GameEvents _events;
    private GameConfig _config;

    public void Initialize(GameEvents events, GameConfig config)
    {
        _events = events;
        _config = config;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasSpawned) return;
        if (!other.CompareTag("SpawnRange")) return;
        if (_enemyPrefab == null)
        {
            Debug.LogError($"[EnemySpawner] No enemy prefab assigned on {gameObject.name}");
            return;
        }

        var enemy = Instantiate(_enemyPrefab, transform.position + _spawnOffset, transform.rotation);
        _hasSpawned = true;

        if (_events != null && _config != null)
        {
            var encounter = enemy.GetComponent<EnemyEncounter>();
            if (encounter != null)
                encounter.Initialize(_events, _config.combatZoomSize, _config.defaultOrthoSize, _config.qteTimeoutSeconds, _config.deathAnimWaitSeconds, _config.slowMotionScale);

            var motor = enemy.GetComponent<CharacterMotor>();
            if (motor != null)
                motor.Initialize(_events, _config.runSpeed);
        }
    }

    public void ResetSpawner()
    {
        _hasSpawned = false;
    }
}
