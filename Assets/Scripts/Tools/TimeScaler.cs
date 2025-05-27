using UnityEngine;


public class TimeScaler : MonoBehaviour
{
    [Range(0.03f, 1.0f)] public float timeCoef;


    void Start()
    {
        SetTimeScale();
    }

    void OnValidate()
    {
        SetTimeScale();
    }

    void SetTimeScale()
    {
        Time.timeScale = timeCoef;
    }
}
