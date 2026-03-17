using System;
using System.Collections;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    private Camera _cam;
    private Coroutine _activeSequence;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    public void Play(CameraKeyframe[] keyframes, Action onComplete = null)
    {
        if (_activeSequence != null)
            StopCoroutine(_activeSequence);

        _activeSequence = StartCoroutine(RunSequence(keyframes, onComplete));
    }

    public void Stop()
    {
        if (_activeSequence != null)
        {
            StopCoroutine(_activeSequence);
            _activeSequence = null;
        }
    }

    private IEnumerator RunSequence(CameraKeyframe[] keyframes, Action onComplete)
    {
        if (keyframes == null || keyframes.Length == 0)
        {
            onComplete?.Invoke();
            yield break;
        }

        transform.position = keyframes[0].position;
        _cam.orthographicSize = keyframes[0].orthographicSize;

        for (int i = 1; i < keyframes.Length; i++)
        {
            var from = keyframes[i - 1];
            var to = keyframes[i];
            float elapsed = 0f;

            while (elapsed < to.duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / to.duration);
                float eased = Ease(t, to.easing);

                transform.position = Vector3.Lerp(from.position, to.position, eased);
                _cam.orthographicSize = Mathf.Lerp(from.orthographicSize, to.orthographicSize, eased);

                yield return null;
            }

            transform.position = to.position;
            _cam.orthographicSize = to.orthographicSize;
        }

        _activeSequence = null;
        onComplete?.Invoke();
    }

    private static float Ease(float t, EasingType type) => type switch
    {
        EasingType.Linear => t,
        EasingType.EaseIn => t * t,
        EasingType.EaseOut => 1f - (1f - t) * (1f - t),
        EasingType.EaseInOut => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f,
        EasingType.Smooth => t * t * (3f - 2f * t),
        _ => t
    };
}
