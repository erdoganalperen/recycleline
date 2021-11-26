using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BasketsController : MonoBehaviour
{
    enum SwipeDirection
    {
        Left,
        Right
    }

    enum BasketDirection
    {
        Left,
        Center,
        Right
    }

    private AudioSource _audioSource;
    public AudioClip failSFX;
    public AudioClip scoreSFX;
    private BasketDirection currentDir;
    private Action<SwipeDirection> OnSwipe;
    public static Action<bool> OnScore;
    private bool moving;
    private int score;
    public Text scoreText;
    //get swerve
    private Vector2 startPos;
    private Vector2 endPos;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        OnSwipe += SwipeBasket;
        OnScore += Score;
        currentDir = BasketDirection.Center;
        moving = false;
        score = 0;
    }

    void SwipeBasket(SwipeDirection dir)
    {
        if (dir == SwipeDirection.Left)
        {
            if (currentDir.Previous() != currentDir && !moving)
            {
                currentDir = currentDir.Previous();
                StartCoroutine(SmoothSwipe(dir));
            }
        }

        if (dir == SwipeDirection.Right)
        {
            if (currentDir.Next() != currentDir&& !moving)
            {
                currentDir = currentDir.Next();
                StartCoroutine(SmoothSwipe(dir));
            }
        }
    }

    IEnumerator SmoothSwipe(SwipeDirection dir)
    {
        Vector3 targetPos = transform.position;
        if (dir==SwipeDirection.Right)
        {
            targetPos.x += (-1.2f);
        }
        else
        {
            targetPos.x += (1.2f);
        }
        while (Mathf.Abs(targetPos.x-transform.position.x)>.1f)
        {
            moving = true;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 20);
            yield return null;
        }

        transform.position = targetPos;
        moving = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var currentSwipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
            currentSwipe.Normalize();
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                OnSwipe?.Invoke(SwipeDirection.Left);
            }

            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                OnSwipe?.Invoke(SwipeDirection.Right);
            }
        }
    }

    void Score(bool sc)
    {
        if (sc)
        {
            score++;
            _audioSource.PlayOneShot(scoreSFX);
        }
        else
        {
            score--;
            _audioSource.PlayOneShot(failSFX);
        }
        scoreText.text = "Score: "+score;
    }
}

public static class Extensions
{
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
        T[] Arr = (T[]) Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (j >= Arr.Length) ? Arr[Arr.Length - 1] : Arr[j];
    }

    public static T Previous<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
        T[] Arr = (T[]) Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (j <= 0) ? Arr[0] : Arr[j];
    }
}