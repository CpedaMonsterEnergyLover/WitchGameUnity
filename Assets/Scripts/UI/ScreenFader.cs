using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public Image img;
    public float duration;

    public void Instant(bool toBlack)
    {
        img.color = new Color(0, 0, 0, toBlack ? 1 : 0);
        gameObject.SetActive(true);
    }
    
    
    
    public async Task FadeScaled(bool toBlack)
    {
        if (Time.timeScale == 0)
        {
            Debug.Log("Time scale is 0");
            return;
        }
        
        gameObject.SetActive(true);
        img.color = new Color(0, 0, 0, toBlack ? 0 : 1);
        
        float t = 0.0f;
        while (t < duration)
        {
            float alpha = toBlack ? t / duration : 1 - t / duration;
            img.color = new Color(0, 0, 0, alpha);
            t += Time.deltaTime;
            await Task.Delay((int)(Time.deltaTime * 1000));
        }
        if(!toBlack) gameObject.SetActive(false);
    }
    
    public async Task FadeUnscaled(bool toBlack)
    {
        img.color = new Color(0, 0, 0, toBlack ? 0 : 1);
        gameObject.SetActive(true);
        float t = 0.0f;
        while (t < duration)
        {
            float alpha = toBlack ? t / duration : 1 - t / duration;
            img.color = new Color(0, 0, 0, alpha);
            t += Time.unscaledDeltaTime;
            await Task.Delay((int)(Time.unscaledDeltaTime * 1000));
        }
        if(!toBlack) gameObject.SetActive(false);
    }

}
