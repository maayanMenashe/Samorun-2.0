using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private GameConfig _config;

    private IGameState _currentState;
    private GameContext _context;

    public GameConfig Config => _config;

    private void Start()
    {
        var bootState = new BootState();
        bootState.Initialize(this, _config);
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void SetContext(GameContext context)
    {
        _context = context;
    }

    public void TransitionTo(IGameState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter(_context);
        _context?.Events.OnStateChanged?.Invoke(_currentState);
    }
}
