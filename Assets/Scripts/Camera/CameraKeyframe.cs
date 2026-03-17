using UnityEngine;

[System.Serializable]
public struct CameraKeyframe
{
    public Vector3 position;
    public float orthographicSize;
    public float duration;
    public EasingType easing;
}

public enum EasingType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    Smooth
}
