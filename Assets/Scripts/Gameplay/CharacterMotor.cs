using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _isEnemy;

    private float _direction;
    private float _baseSpeed;
    private GameEvents _events;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private void Start()
    {
        _direction = _isEnemy ? 1f : -1f;
        _baseSpeed = _speed;
    }

    public void Initialize(GameEvents events, float speed)
    {
        _events = events;
        _speed = speed;
        _baseSpeed = speed;
    }

    private void OnEnable()
    {
        if (_events != null)
        {
            _events.OnCombatStarted += HandleCombatStarted;
            _events.OnCombatEnded += HandleCombatEnded;
        }
    }

    private void OnDisable()
    {
        if (_events != null)
        {
            _events.OnCombatStarted -= HandleCombatStarted;
            _events.OnCombatEnded -= HandleCombatEnded;
        }
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime * Vector3.left);
    }

    private void HandleCombatStarted()
    {
        if (!_isEnemy)
            _speed = 0f;
    }

    private void HandleCombatEnded()
    {
        if (!_isEnemy)
            _speed = _baseSpeed;
    }
}
