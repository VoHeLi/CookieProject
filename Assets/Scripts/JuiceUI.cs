using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceUI : MonoBehaviour
{
    bool outOfBox = false;

    public void ResizeUp()
    {
        outOfBox = false;
        StartCoroutine(ResizeUpAnimation());
    }

    public void ResizeDown()
    {
        outOfBox = true;
        StartCoroutine(ResizeDownAnimation());
    }

    public void ResizeUpAndDown()
    {
        StartCoroutine(ResizeUpAndDownAnimation());
    }

    private IEnumerator ResizeUpAndDownAnimation()
    {
        float time = 0;
        float duration = 0.1f;
        float startScale = 1f;
        float bigScale = 1.1f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(startScale, bigScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        while (time < duration)
        {
            time -= Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(startScale, bigScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    public IEnumerator ResizeUpAnimation()
    {
        //Do a small smooth resize animation to scale up the button a bit
        float time = 0;
        float duration = 0.1f;
        float startScale = 1f;
        float endScale = 1.1f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(startScale, endScale, Mathf.SmoothStep(0.0f,1.0f,t));
            transform.localScale = new Vector3(scale, scale, scale);

            if(outOfBox)
            {
                yield break;
            }

            yield return null;
        }
    }

    public IEnumerator ResizeDownAnimation()
    {
        //Do a small smooth resize animation to scale up the button a bit
        float time = 0;
        float duration = 0.1f;
        float endScale = 1f;
        float startScale = 1.1f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = Mathf.Lerp(startScale, endScale, Mathf.SmoothStep(0.0f, 1.0f, t));
            transform.localScale = new Vector3(scale, scale, scale);

            if (!outOfBox)
            {
                yield break;
            }

            yield return null;
        }
    }
}
