using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings settings;

    public bool EFFECT_TWEENING_ENABLED = false;
    public bool EFFECT_TWEENING_ROTATION = false;
    public bool EFFECT_TWEENING_SCALE = false;

    [Space]
    public float EFFECT_TWEENING_DELAY = 0;
    public float EFFECT_TWEENING_DURATION = 1.5f;

    [Space]
    public bool EFFECT_PADDLE_STRETCH = false;

    [Space]
    public EasingFunction.Ease EASE_FUNCTION = EasingFunction.Ease.EaseOutBounce;



    private void Awake()
    {
        if (settings != null && settings != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            settings = this;
        }
    }
}
