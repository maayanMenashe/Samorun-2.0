using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Vector3 _respawnPoint;
    private GameEvents _events;
    private float _invincibilityDuration;
    private float _invincibleUntil;

    public void Initialize(GameEvents events, float invincibilityDuration)
    {
        _events = events;
        _invincibilityDuration = invincibilityDuration;
        _respawnPoint = transform.position;
    }

    public void Respawn()
    {
        transform.position = _respawnPoint;
        _invincibleUntil = Time.time + _invincibilityDuration;
        _events?.OnPlayerRespawned?.Invoke();
    }

    public void SetRespawnPoint(Vector3 point)
    {
        _respawnPoint = point;
    }

    private bool IsInvincible => Time.time < _invincibleUntil;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (IsInvincible) return;

        _events?.OnPlayerDied?.Invoke();
    }
}
