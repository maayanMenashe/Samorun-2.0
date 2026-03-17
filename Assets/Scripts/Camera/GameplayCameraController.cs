using UnityEngine;

public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed = 7f;
    [SerializeField] private float _defaultOrthoSize = 13f;

    private Camera _cam;
    private bool _snapNextFrame;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        _snapNextFrame = true;
    }

    public void Configure(Transform target, float followSpeed, float defaultOrthoSize)
    {
        _target = target;
        _followSpeed = followSpeed;
        _defaultOrthoSize = defaultOrthoSize;
    }

    public void ResetToDefaults()
    {
        if (_cam != null)
            _cam.orthographicSize = _defaultOrthoSize;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        if (_snapNextFrame)
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            _snapNextFrame = false;
            return;
        }

        Vector2 smoothed = Vector2.Lerp(
            transform.position,
            _target.position,
            _followSpeed * Time.deltaTime);

        transform.position = new Vector3(smoothed.x, smoothed.y, transform.position.z);
    }
}
