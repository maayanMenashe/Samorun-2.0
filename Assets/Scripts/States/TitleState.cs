using System.Collections;
using UnityEngine;

public class TitleState : IGameState
{
    private GameContext _ctx;
    private Coroutine _sequenceCoroutine;
    private Coroutine _fadeOutCoroutine;
    private GameObject _canvasObj;
    private CanvasGroup _titleGroup;
    private CanvasGroup _pressKeyGroup;
    private bool _canPressKey;
    private bool _transitioning;

    public void Enter(GameContext ctx)
    {
        _ctx = ctx;
        _canPressKey = false;
        _transitioning = false;

        _canvasObj = new GameObject("TitleCanvas");
        var canvas = _canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        _canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();

        var titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(canvas.transform, false);
        _titleGroup = titleObj.AddComponent<CanvasGroup>();
        _titleGroup.alpha = 0f;
        var titleText = titleObj.AddComponent<UnityEngine.UI.Text>();
        titleText.text = "SAMORUN";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 80;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        var titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.6f);
        titleRect.anchorMax = new Vector2(0.5f, 0.6f);
        titleRect.sizeDelta = new Vector2(800, 120);

        var pressObj = new GameObject("PressAnyKey");
        pressObj.transform.SetParent(canvas.transform, false);
        _pressKeyGroup = pressObj.AddComponent<CanvasGroup>();
        _pressKeyGroup.alpha = 0f;
        var pressText = pressObj.AddComponent<UnityEngine.UI.Text>();
        pressText.text = "Press any key";
        pressText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        pressText.fontSize = 32;
        pressText.alignment = TextAnchor.MiddleCenter;
        pressText.color = Color.white;
        var pressRect = pressObj.GetComponent<RectTransform>();
        pressRect.anchorMin = new Vector2(0.5f, 0.3f);
        pressRect.anchorMax = new Vector2(0.5f, 0.3f);
        pressRect.sizeDelta = new Vector2(400, 60);

        _sequenceCoroutine = ctx.Flow.StartCoroutine(TitleSequence());
    }

    private IEnumerator TitleSequence()
    {
        float elapsed = 0f;
        float fadeDuration = _ctx.Config.titleFadeDuration;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            _titleGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        _titleGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(_ctx.Config.pressAnyKeyDelay);

        _canPressKey = true;
        while (!_transitioning)
        {
            float pulse = (Mathf.Sin(Time.unscaledTime * 3f) + 1f) / 2f;
            _pressKeyGroup.alpha = 0.3f + pulse * 0.7f;
            yield return null;
        }
    }

    public void Update()
    {
        if (!_canPressKey || _transitioning) return;

        if (Input.anyKeyDown)
        {
            _transitioning = true;
            _fadeOutCoroutine = _ctx.Flow.StartCoroutine(FadeOutAndTransition());
        }
    }

    private IEnumerator FadeOutAndTransition()
    {
        float elapsed = 0f;
        float fadeDuration = _ctx.Config.titleFadeOutDuration;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            _titleGroup.alpha = alpha;
            _pressKeyGroup.alpha = alpha;
            yield return null;
        }

        _ctx.Flow.TransitionTo(new CinematicIntroState());
    }

    public void Exit()
    {
        if (_sequenceCoroutine != null)
            _ctx.Flow.StopCoroutine(_sequenceCoroutine);
        if (_fadeOutCoroutine != null)
            _ctx.Flow.StopCoroutine(_fadeOutCoroutine);

        if (_canvasObj != null)
            Object.Destroy(_canvasObj);
    }
}
