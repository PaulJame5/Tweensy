///<summary> 
/// Updated 18/11/2019 added in beter naming conventions for readability and removed some Mr Grayscale dependant code
/// 
/// 
/// Updated 11/11/2019 to remove dependancies on Mr Grayscale Scripts.
/// Added in better naming conventions for start and end to start value and end value for clarity
/// Removed bug where tween would not loop. Added in more options for tweening in 3 dimensions
/// 
/// Update: 11/11/2019
/// Fixed bugs regarding 3D space for tweening objects
/// 
/// Made for use with Mr. Grayscale. Used Robert Penners functions to 
/// make my own tweening tool for simple animations for use with Unity.
/// 
/// Name Tweensy was used to preent clashes with DoTween in UnityEngine
/// Free to use for whatever reason with no warranty
/// Free to edit and redistribute in anyway you would like that doesn't affect 
/// Robert Penners licenses http://robertpenner.com/easing_terms_of_use.html
/// 
///
///
/// Note to self: could optimise by caching initial images rather 
/// than using get component on each frame avoiding garbage collection build up
/// Bugs: No Known Bugs
/// 
/// </summary>

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
    private const float IN_OUT = .415f;

    // The length of time it takes for an object to get to it's end position
    public float time = 2;
    private float _initial;
    private float _initialEnd;
    public float startValue; 
    public float endValue;
    private float _startTime;



    [HideInInspector]
    public bool isPlaying = false;

    [Range(0, 1)]
    public float opacityTarget;

    public float rotationTarget;
    public float scaleTarget;

    private bool _switchAtEnd = false;


    [Tooltip("If you select curve from the tween option you can select your own tween curve")]
    public AnimationCurve curve;

    public bool play = false;
    public bool loop = false;

    public Transform target;

    private bool _addToTime = false;
    private float _rotatedAt = 0;
    private float _timeToAddOn;
    private bool _setNewTimeSince = false;
    

    public enum ToTween
    {
        POSITION_X,
        POSITION_Y,
        POSITION_Z,
        ROTATION_X,
        ROTATION_Y,
        ROTATION_Z,
        SCALE_X,
        SCALE_Y,
        SCALE_Z,
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
            /*======================== POSITIONS ======================*/
            case ToTween.POSITION_X:
                startValue = transform.localPosition.x;
                endValue = target.transform.localPosition.x;
                break;

            case ToTween.POSITION_Y:
                startValue = transform.localPosition.y;
                endValue = target.transform.localPosition.y;
                break;

            case ToTween.POSITION_Z:
                startValue = transform.localPosition.z;
                endValue = target.transform.localPosition.z;
                break;
            /*======================== POSITIONS ======================*/


            /*======================== SCALE ======================*/
            case ToTween.SCALE_X:
                if (GetComponent<Image>())
                {
                    startValue = transform.GetComponent<Image>().rectTransform.localScale.x;
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(startValue, transform.GetComponent<Image>().rectTransform.localScale.y, transform.GetComponent<Image>().rectTransform.localScale.z);
                }
                else
                {
                    startValue = transform.localScale.x;
                    transform.localScale = new Vector3(startValue, transform.localScale.y, transform.localScale.z);
                }

                endValue = scaleTarget;
                break;

            case ToTween.SCALE_Y:
                if (GetComponent<Image>())
                {
                    startValue = transform.GetComponent<Image>().rectTransform.localScale.y;
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, startValue, transform.GetComponent<Image>().rectTransform.localScale.z);
                }
                else
                {
                    startValue = transform.localScale.y;
                    transform.localScale = new Vector3(transform.localScale.x, startValue, transform.localScale.z);
                }
                endValue = scaleTarget;
                break;

            case ToTween.SCALE_Z:
                if (GetComponent<Image>())
                {
                    startValue = transform.GetComponent<Image>().rectTransform.localScale.z;
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x,transform.GetComponent<Image>().rectTransform.localScale.y,startValue);
                }
                else
                {
                    startValue = transform.localScale.z;
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startValue);
                }
                endValue = scaleTarget;
                break;
            /*======================== SCALE ======================*/


            /*======================== ROTATIONS ======================*/
            case ToTween.ROTATION_X:
                startValue = transform.localEulerAngles.x;
                endValue = rotationTarget;
                break;

            case ToTween.ROTATION_Y:
                startValue = transform.localEulerAngles.y;
                endValue = rotationTarget;
                break;

            case ToTween.ROTATION_Z:
                startValue = transform.localEulerAngles.z;
                endValue = rotationTarget;
                break;
            /*======================== ROTATIONS ======================*/


            /*======================== ALPHA ======================*/
            case ToTween.ALPHA:
                if (GetComponent<SpriteRenderer>())
                {
                    startValue = GetComponent<SpriteRenderer>().color.a;
                }
                if (GetComponent<Image>())
                {
                    startValue = GetComponent<Image>().color.a;
                }
                endValue = opacityTarget;
                break;
            /*======================== ALPHA ======================*/
        }

        // Set for allowing us to loop tweens
        _initial = startValue;
        _initialEnd = endValue;



    }

    // Update is called once per frame
    void Update()
    {
        float temp_value = 0;
        
        if (isPlaying)
        {
            switch (tweenType)
            {
                case Tweens.LINEAR:
                    temp_value = Linear(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUADRATIC_EASE_IN:
                    temp_value = QuadraticEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUADRATIC_EASE_OUT:
                    temp_value = QuadraticEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUADRATIC_EASE_IN_OUT:
                    temp_value = QuadraticEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CUBIC_EASE_IN:
                    temp_value = CubicEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CUBIC_EASE_OUT:
                    temp_value = CubicEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CUBIC_EASE_IN_OUT:
                    temp_value = CubicEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUARTIC_EASE_IN:
                    temp_value = QuarticEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUARTIC_EASE_OUT:
                    temp_value = QuarticEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUARTIC_EASE_IN_OUT:
                    temp_value = QuarticEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUINTIC_EASE_IN:
                    temp_value = QuinticEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUINTIC_EASE_OUT:
                    temp_value = QuinticEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.QUINTIC_EASE_IN_OUT:
                    temp_value = QuarticEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CIRCULAR_EASE_IN:
                    temp_value = CircularEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CIRCULAR_EASE_OUT:
                    temp_value = CircularEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.CIRCULAR_EASE_IN_OUT:
                    temp_value = CircularEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.EXPONENTIAL_EASE_IN:
                    temp_value = ExponentialEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.EXPONENTIAL_EASE_OUT:
                    temp_value = ExponentialEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.EXPONENTIAL_EASE_IN_OUT:
                    temp_value = ExponentialEaseInOut(DeltaTime(), startValue, endValue);
                    break;


                case Tweens.ELASTIC_EASE_IN:
                    temp_value = ElasticEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.ELASTIC_EASE_OUT:
                    temp_value = ElasticEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.ELASTIC_EASE_IN_OUT:
                    temp_value = ElasticEaseInOut(DeltaTime(), startValue, endValue);
                    break;


                case Tweens.BACK_EASE_IN:
                    temp_value = BackEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.BACK_EASE_OUT:
                    temp_value = BackEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.BACK_EASE_IN_OUT:
                    temp_value = BackEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.SINE_EASE_IN:
                    temp_value = SineEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.SINE_EASE_OUT:
                    temp_value = SineEaseOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.SINE_EASE_IN_OUT:
                    temp_value = SineEaseInOut(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.BOUNCE_EASE_IN:
                    temp_value = BounceEaseIn(DeltaTime(), startValue, endValue);
                    break;

                case Tweens.BOUNCE_EASE_OUT:
                    temp_value = BounceEaseOut(DeltaTime(), false, startValue, endValue);
                    break;

                case Tweens.BOUNCE_EASE_IN_OUT:
                    temp_value = BounceEaseInOut(DeltaTime(), startValue, endValue);
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
                /*==============================POSITIONS================================*/
                case ToTween.POSITION_X:
                    transform.localPosition = new Vector3(temp_value, transform.localPosition.y, transform.localPosition.z);
                    break;

                case ToTween.POSITION_Y:
                    transform.localPosition = new Vector3(transform.localPosition.x, temp_value, transform.localPosition.z);
                    break;

                case ToTween.POSITION_Z:
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, temp_value);
                    break;
                /*==============================POSITIONS================================*/



                /*==============================SCALE================================*/
                case ToTween.SCALE_X:
                    if (GetComponent<Image>())
                    {
                        transform.GetComponent<Image>().rectTransform.localScale = new Vector3(temp_value, transform.GetComponent<Image>().rectTransform.localScale.y, transform.GetComponent<Image>().rectTransform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(temp_value, transform.localScale.y, transform.localScale.z);
                    }
                    break;

                case ToTween.SCALE_Y:
                    if (GetComponent<Image>())
                    {
                        transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x,temp_value, transform.GetComponent<Image>().rectTransform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x,temp_value, transform.localScale.z);
                    }
                    break;

                case ToTween.SCALE_Z:
                    if (GetComponent<Image>())
                    {
                        transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, transform.GetComponent<Image>().rectTransform.localScale.y,temp_value);
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, temp_value);
                    }
                    break;
                /*==============================SCALE================================*/

                /*==============================ROTATION================================*/
                case ToTween.ROTATION_Z:
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, temp_value);

                    break;
                case ToTween.ROTATION_Y:
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, temp_value, transform.localEulerAngles.z);

                    break;
                case ToTween.ROTATION_X:
                    transform.localEulerAngles = new Vector3(temp_value, transform.localEulerAngles.y, transform.localEulerAngles.z);

                    break;

                /*==============================ROTATION================================*/

                /*==============================ALPHA================================*/
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
                    /*==============================ALPHA================================*/
            }

            // if the absolute value or closest to is reached we set isPlaying to false
            if (System.Math.Abs(temp_value - endValue) < Mathf.Epsilon)
            {
                isPlaying = false;
            }
            

        } // end if is playing


       

        // switch at end to change state from enter to exit
        if (!isPlaying && _switchAtEnd)
        {
            _switchAtEnd = false;
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

        // if play selected and not playing or is not playing and looping
        if ((!isPlaying && loop) || (play && !isPlaying))
        {
            _switchAtEnd = true;
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
        float timeDelta = Time.timeSinceLevelLoad - _startTime;

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

        _startTime = Time.timeSinceLevelLoad;
    }

    void ExitAnimation()
    {
        tweenType = exitTween;
        endValue = _initial;
        startValue = _initialEnd;
    }
    void EnterAnimation()
    {
        tweenType = enteringTween;
        endValue = _initialEnd;
        startValue = _initial;
    }


    void ReplayAnimation()
    {

        // Assign Updated Values
        switch (toTween)
        {

            /*======================== POSITIONS ======================*/
            case ToTween.POSITION_X:
                transform.localPosition = new Vector3(startValue, transform.localPosition.y, transform.localPosition.z);
                break;

            case ToTween.POSITION_Y:
                transform.localPosition = new Vector3(transform.localPosition.x, startValue, transform.localPosition.z);
                break;

            case ToTween.POSITION_Z:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startValue);
                break;
            /*======================== POSITIONS ======================*/




            /*======================== SCALE ======================*/
            case ToTween.SCALE_X:
                if (GetComponent<Image>())
                {
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(startValue, transform.GetComponent<Image>().rectTransform.localScale.y);
                }
                else
                {
                    transform.localScale = new Vector3(startValue, transform.localScale.y, transform.localScale.z);
                }
                break;

            case ToTween.SCALE_Y:
                if (GetComponent<Image>())
                {
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, startValue);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, startValue, transform.localScale.z);
                }
                break;

            case ToTween.SCALE_Z:
                if (GetComponent<Image>())
                {
                    transform.GetComponent<Image>().rectTransform.localScale = new Vector3(transform.GetComponent<Image>().rectTransform.localScale.x, transform.GetComponent<Image>().rectTransform.localScale.y, startValue);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startValue);
                }
                break;
            /*======================== SCALE ======================*/



            /*======================== ROTATION ======================*/
            case ToTween.ROTATION_Z:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, startValue);

                break;
            case ToTween.ROTATION_Y:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,startValue, transform.localEulerAngles.z);

                break;
            case ToTween.ROTATION_X:
                transform.localEulerAngles = new Vector3(startValue, transform.localEulerAngles.y, transform.localEulerAngles.z);

                break;
            /*======================== ROTATION ======================*/


            /*======================== ALPHA ======================*/
            case ToTween.ALPHA:
                if (GetComponent<SpriteRenderer>())
                {
                    Color temp_color = GetComponent<SpriteRenderer>().color;
                    temp_color.a = startValue;
                    GetComponent<SpriteRenderer>().color = temp_color;
                }
                else if (GetComponent<Image>())
                {
                    Color temp_color = GetComponent<Image>().color;
                    temp_color.a = startValue;
                    GetComponent<Image>().color = temp_color;
                }
                break;
             /*======================== ALPHA ======================*/
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
        return (endValue - startValue) * curve.Evaluate(t_delta) + startValue;
    }


}
