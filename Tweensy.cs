///<summary> 
/// Updated: 11/11/2019 Removed Mr. Grayscale dependencies to work as a stand alone tween script
/// Made for use with Mr. Grayscale. Used Robert Penners functions to make my own tweening tool for simple animations for use with Unity. 
/// Name Tweensy was used to preent clashes with DoTween in UnityEngine
/// Free to use for whatever reason with no warranty
/// Free to edit and redistribute in anyway you would like that doesn't affect Robert Penners licenses http://robertpenner.com/easing_terms_of_use.html
/// </summary>


// Note to self: could optimise by caching initial images rather than using get component on each frame avoiding garbage collection build up
// Bugs: No Known Bugs

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tweensy : MonoBehaviour
{
    /// <summary>
    /// Constant Pi.
    /// </summary>
    private const float PI = Mathf.PI;

    /// <summary>
    /// Constant Pi / 2.
    /// </summary>
    private const float HALFPI = Mathf.PI / 2.0f;

    /// <summary>
    /// Used for smoothing out bounceeaseinout
    /// </summary>
    private readonly float IN_OUT = .415f;

    public float time = 2;
    private float initial;
    private float initialEnd;
    public float start;
    public float end;
    private float startTime;



    [HideInInspector]
    public bool isPlaying = false;

    [Range(0, 1)]
    public float opacityTarget;

    public float rotationTarget;
    public float scaleTarget;

    bool switchAtEnd = false;


    [Tooltip("If you select curve from the tween option you can select your own tween curve")]
    public AnimationCurve curve;

    public bool play = false;
    public bool loop = false;

    public Transform target;

    private bool addToTime = false;
    private float rotatedAt = 0;
    private float timeToAddOn;
    bool setNewTimeSince = false;

    [Space(10)]
    public bool isPillar = false;

    public enum ToTween
    {
        POSITION_X,
        POSITION_Y,
        ROTATION_Z,
        SCALE_X,
        SCALE_Y,
        ALPHA
    }

    public enum Tweens
    {
        LINEAR,
        QUADRATIC_EASE_IN,
        QUADRATIC_EASE_OUT,
        QUADRATIC_EASE_IN_OUT,
        CUBIC_EASE_IN,
        CUBIC_EASE_OUT,
        CUBIC_EASE_IN_OUT,
        QUARTIC_EASE_IN,
        QUARTIC_EASE_OUT,
        QUARTIC_EASE_IN_OUT,
        QUINTIC_EASE_IN,
        QUINTIC_EASE_OUT,
        QUINTIC_EASE_IN_OUT,
        CIRCULAR_EASE_IN,
        CIRCULAR_EASE_OUT,
        CIRCULAR_EASE_IN_OUT,
        EXPONENTIAL_EASE_IN,
        EXPONENTIAL_EASE_OUT,
        EXPONENTIAL_EASE_IN_OUT,
        ELASTIC_EASE_IN,
        ELASTIC_EASE_OUT,
        ELASTIC_EASE_IN_OUT,
        BACK_EASE_IN,
        BACK_EASE_OUT,
        BACK_EASE_IN_OUT,
        SINE_EASE_IN,
        SINE_EASE_OUT,
        SINE_EASE_IN_OUT,
        BOUNCE_EASE_IN,
        BOUNCE_EASE_OUT,
        BOUNCE_EASE_IN_OUT,
        CURVE
    }

    public enum State
    {
        ENTER,
        EXIT
    }


    public ToTween toTween;
    private Tweens tweenType;
    public Tweens enteringTween;
    public Tweens exitTween;
    public State state;

    // Use this for initialization
    void Start()
    {
        tweenType = enteringTween;

        switch (toTween)
        {
            case ToTween.POSITION_X:
                start = transform.localPosition.x;
                end = target.transform.localPosition.x;
                break;

            case ToTween.POSITION_Y:
                start = transform.localPosition.y;
                end = target.transform.localPosition.y;
                break;

            case ToTween.SCALE_X:
                if (GetComponent<Image>())
                {
                    start = transform.GetComponent<Image>().rectTransform.localScale.x;
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(start, transform.GetComponent<Image>().rectTransform.localScale.y, transform.GetComponent<Image>().rectTransform.localScale.z);
                }
                else
                {
                    start = transform.localScale.x;
                    transform.localScale = new Vector3(start, transform.localScale.y, transform.localScale.z);
                }

                end = scaleTarget;
                break;

            case ToTween.SCALE_Y:
                if (GetComponent<Image>())
                {
                    start = transform.GetComponent<Image>().rectTransform.localScale.y;
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, start, transform.GetComponent<Image>().rectTransform.localScale.z);
                }
                else
                {
                    start = transform.localScale.y;
                    transform.localScale = new Vector3(transform.localScale.x, start, transform.localScale.z);
                }
                end = scaleTarget;
                break;

            case ToTween.ROTATION_Z:
                start = transform.localEulerAngles.z;
                end = rotationTarget;
                break;


            case ToTween.ALPHA:
                if (GetComponent<SpriteRenderer>())
                {
                    start = GetComponent<SpriteRenderer>().color.a;
                    end = opacityTarget;
                }
                if (GetComponent<Image>())
                {
                    start = GetComponent<Image>().color.a;
                    end = opacityTarget;
                }
                break;
        }
        initial = start;
        initialEnd = end;



    }

    // Update is called once per frame
    void Update()
    {
        float temp_value = 0;

        // This can be removed for your own projects as this was made to work with Mr. Grayscale
        if (addToTime)
        {
            timeToAddOn = Time.timeSinceLevelLoad - rotatedAt;
            startTime += timeToAddOn;
            setNewTimeSince = false;
            addToTime = false;
        }
        // World Rotation can be removed to suit your own project needs
        else if (isPlaying)
        {
            switch (tweenType)
            {
                case Tweens.LINEAR:
                    temp_value = Linear(DeltaTime(), start, end);
                    break;

                case Tweens.QUADRATIC_EASE_IN:
                    temp_value = QuadraticEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.QUADRATIC_EASE_OUT:
                    temp_value = QuadraticEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.QUADRATIC_EASE_IN_OUT:
                    temp_value = QuadraticEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.CUBIC_EASE_IN:
                    temp_value = CubicEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.CUBIC_EASE_OUT:
                    temp_value = CubicEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.CUBIC_EASE_IN_OUT:
                    temp_value = CubicEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.QUARTIC_EASE_IN:
                    temp_value = QuarticEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.QUARTIC_EASE_OUT:
                    temp_value = QuarticEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.QUARTIC_EASE_IN_OUT:
                    temp_value = QuarticEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.QUINTIC_EASE_IN:
                    temp_value = QuinticEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.QUINTIC_EASE_OUT:
                    temp_value = QuinticEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.QUINTIC_EASE_IN_OUT:
                    temp_value = QuarticEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.CIRCULAR_EASE_IN:
                    temp_value = CircularEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.CIRCULAR_EASE_OUT:
                    temp_value = CircularEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.CIRCULAR_EASE_IN_OUT:
                    temp_value = CircularEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.EXPONENTIAL_EASE_IN:
                    temp_value = ExponentialEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.EXPONENTIAL_EASE_OUT:
                    temp_value = ExponentialEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.EXPONENTIAL_EASE_IN_OUT:
                    temp_value = ExponentialEaseInOut(DeltaTime(), start, end);
                    break;


                case Tweens.ELASTIC_EASE_IN:
                    temp_value = ElasticEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.ELASTIC_EASE_OUT:
                    temp_value = ElasticEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.ELASTIC_EASE_IN_OUT:
                    temp_value = ElasticEaseInOut(DeltaTime(), start, end);
                    break;


                case Tweens.BACK_EASE_IN:
                    temp_value = BackEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.BACK_EASE_OUT:
                    temp_value = BackEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.BACK_EASE_IN_OUT:
                    temp_value = BackEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.SINE_EASE_IN:
                    temp_value = SineEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.SINE_EASE_OUT:
                    temp_value = SineEaseOut(DeltaTime(), start, end);
                    break;

                case Tweens.SINE_EASE_IN_OUT:
                    temp_value = SineEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.BOUNCE_EASE_IN:
                    temp_value = BounceEaseIn(DeltaTime(), start, end);
                    break;

                case Tweens.BOUNCE_EASE_OUT:
                    temp_value = BounceEaseOut(DeltaTime(), false, start, end);
                    break;

                case Tweens.BOUNCE_EASE_IN_OUT:
                    temp_value = BounceEaseInOut(DeltaTime(), start, end);
                    break;

                case Tweens.CURVE:
                    temp_value = Curve(DeltaTime());
                    break;

                default:
                    temp_value = 0;
                    break;
            }


            // Assign Updated Values
            switch (toTween)
            {
                case ToTween.POSITION_X:
                    transform.localPosition = new Vector2(temp_value, transform.localPosition.y);
                    break;

                case ToTween.POSITION_Y:
                    transform.localPosition = new Vector2(transform.localPosition.x, temp_value);
                    break;

                case ToTween.SCALE_X:
                    if (GetComponent<Image>())
                    {
                        transform.GetComponent<Image>().rectTransform.localScale = new Vector3(temp_value, transform.GetComponent<Image>().rectTransform.localScale.y);
                    }
                    else
                    {
                        transform.localScale = new Vector2(temp_value, transform.localScale.y);
                    }
                    break;

                case ToTween.SCALE_Y:
                    if (GetComponent<Image>())
                    {
                        transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, temp_value);
                    }
                    else
                    {
                        transform.localScale = new Vector2(transform.localScale.x, temp_value);
                    }
                    break;

                case ToTween.ROTATION_Z:
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, temp_value);

                    break;


                case ToTween.ALPHA:
                    if (GetComponent<SpriteRenderer>())
                    {
                        Color temp_color = GetComponent<SpriteRenderer>().color;
                        temp_color.a = temp_value;
                        GetComponent<SpriteRenderer>().color = temp_color;
                    }
                    else if (GetComponent<Image>())
                    {
                        Color temp_color = GetComponent<Image>().color;
                        temp_color.a = temp_value;
                        GetComponent<Image>().color = temp_color;
                    }
                    break;
            }

            if (System.Math.Abs(temp_value - end) < Mathf.Epsilon)
            {
                isPlaying = false;
            }

            switch (toTween)
            {
                case ToTween.ROTATION_Z:
                    if (System.Math.Abs(temp_value - end) < Mathf.Epsilon)
                    {
                        if (isPlaying)
                        {
                            Debug.Log("workis");
                            //temp_value = 0;
                            isPlaying = false;
                        }
                    }
                    break;
            }

        } // end if is playing && !rotating


       

        // switch at end to change state from enter to exit
        if (!isPlaying && switchAtEnd)
        {
            switchAtEnd = false;
            switch (state)
            {
                case State.ENTER:
                    state = State.EXIT;
                    break;
                case State.EXIT:
                    state = State.ENTER;
                    break;
            }
        }

        if (play && !isPlaying || loop)
        {
            switchAtEnd = true;
            Bounce();

            foreach (Tweensy tween in GetComponents<Tweensy>())
            {
                if (!tween.play)
                {
                    tween.play = true;
                }
            }
            play = false;
            isPlaying = true;
        }

        if (isPlaying)
        {
            play = false;
        }

    }




    float DeltaTime()
    {
        float timeDelta = Time.timeSinceLevelLoad - startTime;

        if (timeDelta < time)
        {
            return timeDelta / time;
        }
        else
        {

            return 1;
        }
    }

    void Bounce()
    {

        if (System.Math.Abs(DeltaTime() - 1) < Mathf.Epsilon)
        {

            switch (state)
            {
                case State.ENTER:
                    EnterAnimation();


                    break;
                case State.EXIT:
                    ExitAnimation();
                    break;
            }
            ReplayAnimation();
        }

        startTime = Time.timeSinceLevelLoad;
    }

    void ExitAnimation()
    {
        tweenType = exitTween;
        end = initial;
        start = initialEnd;
    }
    void EnterAnimation()
    {
        tweenType = enteringTween;
        end = initialEnd;
        start = initial;
    }
    void ReplayAnimation()
    {

        // Assign Updated Values
        switch (toTween)
        {
            case ToTween.POSITION_X:
                transform.localPosition = new Vector2(start, transform.localPosition.y);
                break;

            case ToTween.POSITION_Y:
                transform.localPosition = new Vector2(transform.localPosition.x, start);
                break;

            case ToTween.SCALE_X:
                if (GetComponent<Image>())
                {
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(start, transform.GetComponent<Image>().rectTransform.localScale.y);
                }
                else
                {
                    transform.localScale = new Vector2(start, transform.localScale.y);
                }
                break;

            case ToTween.SCALE_Y:
                if (GetComponent<Image>())
                {
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, start);
                }
                else
                {
                    transform.localScale = new Vector2(transform.localScale.x, start);
                }
                break;

            case ToTween.ROTATION_Z:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, start);

                break;


            case ToTween.ALPHA:
                if (GetComponent<SpriteRenderer>())
                {
                    Color temp_color = GetComponent<SpriteRenderer>().color;
                    temp_color.a = start;
                    GetComponent<SpriteRenderer>().color = temp_color;
                }
                else if (GetComponent<Image>())
                {
                    Color temp_color = GetComponent<Image>().color;
                    temp_color.a = start;
                    GetComponent<Image>().color = temp_color;
                }
                break;
        }
    }

    float Linear(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, t_delta);
    }

    float QuadraticEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, t_delta * t_delta);
    }

    //-(x * (x - 2))
    float QuadraticEaseOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, -(t_delta * (t_delta - 2)));
    }

    /// <summary>
    /// Modeled after the piecewise quadratic
    /// y = (1/2)((2x)^2)             ; [0, 0.5)
    /// y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
    /// </summary>
    public float QuadraticEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 2 * t_delta * t_delta);
        }
        else
        {
            return Mathf.LerpUnclamped(t_start, t_end, (-2 * t_delta * t_delta) + (4 * t_delta) - 1);
        }
    }
    /// <summary>
    /// Modeled after quarter-cycle of sine wave
    /// </summary>
    public float SineEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Sin((t_delta - 1) * HALFPI) + 1);
    }

    /// <summary>
    /// Modeled after quarter-cycle of sine wave (different phase)
    /// </summary>
    public float SineEaseOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Sin(t_delta * HALFPI));
    }

    /// <summary>
    /// Modeled after half sine wave
    /// </summary>
    public float SineEaseInOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (1 - Mathf.Cos(t_delta * PI)));
    }

    /// <summary>
    /// Modeled after the cubic y = x^3
    /// </summary>
    public float CubicEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(t_delta, 3));
    }

    /// <summary>
    /// Modeled after the cubic y = (x - 1)^3 + 1
    /// </summary>
    public float CubicEaseOut(float t_delta, float t_start, float t_end)
    {
        float temp_val = (t_delta - 1);
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(temp_val, 3) + 1);
    }

    /// <summary>   
    /// Modeled after the piecewise cubic
    /// y = (1/2)((2x)^3)       ; [0, 0.5)
    /// y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
    /// </summary>
    public float CubicEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 4 * Mathf.Pow(t_delta, 3));
        }
        else
        {
            float temp_val = ((2 * t_delta) - 2);
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * Mathf.Pow(temp_val, 3) + 1);
        }
    }

    /// <summary>
    /// Modeled after the quartic x^4
    /// </summary>
    public float QuarticEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(t_delta, 4));
    }

    /// <summary>
    /// Modeled after the quartic y = 1 - (x - 1)^4
    /// </summary>
    public float QuarticEaseOut(float t_delta, float t_start, float t_end)
    {
        float temp_value = (t_delta - 1);

        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(temp_value, 3) * (1 - t_delta) + 1);
    }

    /// <summary>
    /// Modeled after the piecewise quartic
    /// y = (1/2)((2x)^4)        ; [0, 0.5)
    /// y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
    /// </summary>
    public float QuarticEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 8 * Mathf.Pow(t_delta, 4));
        }
        else
        {
            float temp_val = (t_delta - 1);
            return Mathf.LerpUnclamped(t_start, t_end, -8 * Mathf.Pow(temp_val, 4) + 1);
        }
    }

    /// <summary>
    /// Modeled after the quintic y = x^5
    /// </summary>
    public float QuinticEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(t_delta, 5));
    }

    /// <summary>
    /// Modeled after the quintic y = (x - 1)^5 + 1
    /// </summary>
    public float QuinticEaseOut(float t_delta, float t_start, float t_end)
    {
        float temp_val = (t_delta - 1);

        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(temp_val, 5) + 1);
    }

    /// <summary>
    /// Modeled after the piecewise quintic
    /// y = (1/2)((2x)^5)       ; [0, 0.5)
    /// y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
    /// </summary>
    public float QuinticEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 16 * Mathf.Pow(t_delta, 5));
        }
        else
        {
            float temp_val = ((2 * t_delta) - 2);
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * Mathf.Pow(temp_val, 5) + 1);
        }
    }


    /// <summary>
    /// Modeled after shifted quadrant IV of unit circle
    /// </summary>
    public float CircularEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, 1 - Mathf.Sqrt(1 - (t_delta * t_delta)));
    }

    /// <summary>
    /// Modeled after shifted quadrant II of unit circle
    /// </summary>
    public float CircularEaseOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Sqrt((2 - t_delta) * t_delta));
    }

    /// <summary>   
    /// Modeled after the piecewise circular function
    /// y = (1/2)(1 - Math.Sqrt(1 - 4x^2))           ; [0, 0.5)
    /// y = (1/2)(Math.Sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
    /// </summary>
    public float CircularEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (1 - Mathf.Sqrt(1 - 4 * (t_delta * t_delta))));
        }
        else
        {
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (Mathf.Sqrt(-((2 * t_delta) - 3) * ((2 * t_delta) - 1)) + 1));
        }
    }

    /// <summary>
    /// Modeled after the exponential function y = 2^(10(x - 1))
    /// </summary>
    public float ExponentialEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, (System.Math.Abs(t_delta) < Mathf.Epsilon) ? t_delta : Mathf.Pow(2, 10 * (t_delta - 1)));
    }

    /// <summary>
    /// Modeled after the exponential function y = -2^(-10x) + 1
    /// </summary>
    public float ExponentialEaseOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, (System.Math.Abs(t_delta - 1.0f) < Mathf.Epsilon) ? t_delta : 1 - Mathf.Pow(2, -10 * t_delta));
    }

    /// <summary>
    /// Modeled after the piecewise exponential
    /// y = (1/2)2^(10(2x - 1))         ; [0,0.5)
    /// y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
    /// </summary>
    public float ExponentialEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (System.Math.Abs(t_delta) < Mathf.Epsilon || System.Math.Abs(t_delta - 1.0) < Mathf.Epsilon)
        {
            return t_delta;
        }


        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * Mathf.Pow(2, (20 * t_delta) - 10));
        }
        else
        {
            return Mathf.LerpUnclamped(t_start, t_end, -0.5f * Mathf.Pow(2, (-20 * t_delta) + 10) + 1);
        }
    }

    /// <summary>
    /// Modeled after the damped sine wave y = sin(13pi/2*x)*Math.Pow(2, 10 * (x - 1))
    /// </summary>
    public float ElasticEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Sin(13 * HALFPI * t_delta) * Mathf.Pow(2, 10 * (t_delta - 1)));
    }

    /// <summary>
    /// Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*Math.Pow(2, -10x) + 1
    /// </summary>
    public float ElasticEaseOut(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Sin(-13 * HALFPI * (t_delta + 1)) * Mathf.Pow(2, -10 * t_delta) + 1);
    }

    /// <summary>
    /// Modeled after the piecewise exponentially-damped sine wave:
    /// y = (1/2)*sin(13pi/2*(2*x))*Math.Pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
    /// y = (1/2)*(sin(-13pi/2*((2x-1)+1))*Math.Pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
    /// </summary>
    public float ElasticEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * Mathf.Sin(13 * HALFPI * (2 * t_delta)) * Mathf.Pow(2, 10 * ((2 * t_delta) - 1)));
        }
        else
        {
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (Mathf.Sin(-13 * HALFPI * ((2 * t_delta - 1) + 1)) * Mathf.Pow(2, -10 * (2 * t_delta - 1)) + 2));
        }
    }

    /// <summary>
    /// Modeled after the overshooting cubic y = x^3-x*sin(x*pi)
    /// </summary>
    public float BackEaseIn(float t_delta, float t_start, float t_end)
    {
        return Mathf.LerpUnclamped(t_start, t_end, Mathf.Pow(t_delta, 3) - t_delta * Mathf.Sin(t_delta * PI));
    }

    /// <summary>
    /// Modeled after overshooting cubic y = 1-((1-x)^3-(1-x)*sin((1-x)*pi))
    /// </summary>  
    public float BackEaseOut(float t_delta, float t_start, float t_end)
    {
        //float c = t_start - t_end;
        //float t = t_delta;
        //float s = 1.70158f;
        //float d = 2;
        //return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + t_start;
        ////(s + 1) * t ^ 3 - s * t ^ 2) 
        float temp_val = (1 - t_delta);
        return Mathf.LerpUnclamped(t_start, t_end, 1 - (Mathf.Pow(temp_val, 3) - temp_val * Mathf.Sin(temp_val * PI)));
    }

    /// <summary>
    /// Modeled after the piecewise overshooting cubic function:
    /// y = (1/2)*((2x)^3-(2x)*sin(2*x*pi))           ; [0, 0.5)
    /// y = (1/2)*(1-((1-x)^3-(1-x)*sin((1-x)*pi))+1) ; [0.5, 1]
    /// </summary>
    public float BackEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < 0.5f)
        {
            float temp_val = 2 * t_delta;
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (Mathf.Pow(temp_val, 3) - temp_val * Mathf.Sin(temp_val * PI)));
        }
        else
        {
            float temp_val = (1 - (2 * t_delta - 1));
            return Mathf.LerpUnclamped(t_start, t_end, 0.5f * (1 - (Mathf.Pow(temp_val, 3) - temp_val * Mathf.Sin(temp_val * PI))) + 0.5f);
        }
    }

    /// <summary>
    /// </summary>
    public float BounceEaseIn(float t_delta, float t_start, float t_end)
    {
        return BounceEaseOut(1 - t_delta, true, t_start, t_end);
    }


    /// <summary>
    /// Bounces the ease out.
    /// </summary>
    /// <returns>The ease out.</returns>
    /// <param name="t_delta">T delta.</param>
    /// <param name="t_easeIn">If set to <c>true</c>ease in.</param>
    /// <param name="t_start">T start.</param>
    /// <param name="t_end">T end.</param>
    public float BounceEaseOut(float t_delta, bool t_easeIn, float t_start, float t_end)
    {
        if (t_delta < 4 / 11.0f)
        {
            if (t_easeIn)
            {
                return Mathf.LerpUnclamped(t_end, t_start, ((121.0f * t_delta * t_delta) / 16));
            }
            return Mathf.LerpUnclamped(t_start, t_end, ((121.0f * t_delta * t_delta) / 16));
        }
        else if (t_delta < 8 / 11.0f)
        {
            if (t_easeIn)
            {
                return Mathf.LerpUnclamped(t_end, t_start, ((363 / 40.0f * t_delta * t_delta) - (99 / 10.0f * t_delta) + 17 / 5.0f));
            }
            return Mathf.LerpUnclamped(t_start, t_end, ((363 / 40.0f * t_delta * t_delta) - (99 / 10.0f * t_delta) + 17 / 5.0f));
        }
        else if (t_delta < 9 / 10.0f)
        {
            if (t_easeIn)
            {
                return Mathf.LerpUnclamped(t_end, t_start, ((4356 / 361.0f * t_delta * t_delta) - (35442 / 1805.0f * t_delta) + 16061 / 1805.0f));
            }
            return Mathf.LerpUnclamped(t_start, t_end, ((4356 / 361.0f * t_delta * t_delta) - (35442 / 1805.0f * t_delta) + 16061 / 1805.0f));
        }
        else
        {
            if (t_easeIn)
            {
                return Mathf.LerpUnclamped(t_end, t_start, ((54 / 5.0f * t_delta * t_delta) - (513 / 25.0f * t_delta) + 268 / 25.0f));
            }
            return Mathf.LerpUnclamped(t_start, t_end, ((54 / 5.0f * t_delta * t_delta) - (513 / 25.0f * t_delta) + 268 / 25.0f));
        }
    }
    /// <summary>
    /// Bounce out and land with a bounce
    /// </summary>
    public float BounceEaseInOut(float t_delta, float t_start, float t_end)
    {
        if (t_delta < IN_OUT)
        {
            return BounceEaseIn(t_delta * 2, t_start, t_end);
        }
        else
        {
            return (BounceEaseOut(t_delta, false, t_start, t_end));
        }

    }


    /// <summary>
    /// Curve the specified t_delta. Used within Unity for visualised graph can be kept in or removed depending on your project
    /// </summary>
    /// <returns>The curve.</returns>
    /// <param name="t_delta">T delta.</param>
    private float Curve(float t_delta)
    {
        return (end - start) * curve.Evaluate(t_delta) + start;
    }


}
