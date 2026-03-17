using System;
using System.Collections;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private float _battleAreaRadius = 5f;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private KeyCode[] _qteSequence = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    private Camera _cam;
    private Animator _animator;
    private CircleCollider2D _battleCollider;
    private Vector3 _arrowOriginalScale;
    private Coroutine _fightCoroutine;

    private GameEvents _events;
    private float _combatZoomSize;
    private float _defaultOrthoSize;
    private float _qteTimeout;
    private float _deathAnimWait;
    private float _slowMotionScale;

    private void Start()
    {
        _cam = Camera.main;
        _battleCollider = GetComponentInChildren<CircleCollider2D>();
        if (_battleCollider != null)
            _battleCollider.radius = _battleAreaRadius;
        _animator = GetComponent<Animator>();
        if (_animator != null)
            _animator.SetInteger("AnimationNum", -1);
        if (_arrow != null)
            _arrowOriginalScale = _arrow.transform.localScale;
    }

    public void Initialize(GameEvents events, float combatZoomSize, float defaultOrthoSize, float qteTimeout, float deathAnimWait, float slowMotionScale)
    {
        _events = events;
        _combatZoomSize = combatZoomSize;
        _defaultOrthoSize = defaultOrthoSize;
        _qteTimeout = qteTimeout;
        _deathAnimWait = deathAnimWait;
        _slowMotionScale = slowMotionScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (_fightCoroutine != null) return;

        _fightCoroutine = StartCoroutine(Fight());
    }

    private IEnumerator Fight()
    {
        _events?.OnCombatStarted?.Invoke();

        _cam.orthographicSize = _combatZoomSize;
        Time.timeScale = _slowMotionScale;

        for (int i = 0; i < _qteSequence.Length; i++)
        {
            if (_animator != null)
                _animator.SetInteger("AnimationNum", i - 1);
            _cam.orthographicSize = _combatZoomSize + (_qteSequence.Length - 1 - i);

            FlipArrow(_qteSequence[i]);

            float waited = 0f;
            bool pressed = false;
            while (!pressed)
            {
                if (Input.GetKeyDown(_qteSequence[i]))
                {
                    pressed = true;
                }
                else
                {
                    waited += Time.unscaledDeltaTime;
                    if (waited >= _qteTimeout)
                    {
                        QteFailed();
                        yield break;
                    }
                    yield return null;
                }
            }
        }

        if (_arrow != null)
            _arrow.SetActive(false);

        var motor = GetComponent<CharacterMotor>();
        if (motor != null) motor.Speed = 0f;

        if (_animator != null)
            _animator.SetTrigger("Dead");

        yield return new WaitForSecondsRealtime(_deathAnimWait);

        Time.timeScale = 1f;
        _cam.orthographicSize = _defaultOrthoSize;
        _events?.OnCombatEnded?.Invoke();
        _events?.OnEnemyKilled?.Invoke();

        Destroy(gameObject);
    }

    private void QteFailed()
    {
        Time.timeScale = 1f;
        _cam.orthographicSize = _defaultOrthoSize;
        if (_arrow != null)
            _arrow.SetActive(false);
        _events?.OnCombatEnded?.Invoke();
        _fightCoroutine = null;
    }

    public void ForceStopCombat()
    {
        if (_fightCoroutine != null)
        {
            StopCoroutine(_fightCoroutine);
            _fightCoroutine = null;
        }
        Time.timeScale = 1f;
        if (_cam != null)
            _cam.orthographicSize = _defaultOrthoSize;
        _events?.OnCombatEnded?.Invoke();
    }

    private void FlipArrow(KeyCode input)
    {
        if (_arrow == null) return;

        int hDir = input == KeyCode.RightArrow ? -1 : 1;
        int vDir = input == KeyCode.DownArrow ? -1 : 1;

        _arrow.transform.localScale = new Vector3(
            hDir * _arrowOriginalScale.x,
            vDir * _arrowOriginalScale.y,
            _arrowOriginalScale.z);
    }

    private void OnDestroy()
    {
        if (_fightCoroutine != null)
        {
            Time.timeScale = 1f;
        }
    }
}
