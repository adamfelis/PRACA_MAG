using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class EnterEffectPanel : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private const byte maxImageAlphaChannel = 255;
    private const byte minImageAlphaChannel = 100;
    private byte desiredAlphaChannel;

    private Image image;
    private float elapsedTime;
    private const float animationTime = 0.5f;
    private bool animationPending = false;

    private void Awake()
    {
        image = transform.parent.FindChild("Panel_background").GetComponentInParent<Image>();
        elapsedTime = 0;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        desiredAlphaChannel = maxImageAlphaChannel;
        StartCoroutine(changeAlphaChannel());
    }

    public void OnPointerExit(PointerEventData data)
    {
        StopAllCoroutines();
        elapsedTime = 0;
        desiredAlphaChannel = minImageAlphaChannel;
        StartCoroutine(changeAlphaChannel());
    }

    private IEnumerator changeAlphaChannel()
    {
        float fraction = 0;
        Color capturedColor = image.color;
        float capturedAlpha = capturedColor.a;
        while (fraction < 1)
        {
            fraction = elapsedTime/animationTime;
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(capturedAlpha, desiredAlphaChannel / 255f, fraction);
            capturedColor.a = newAlpha;
            image.color = capturedColor;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
    }
}
