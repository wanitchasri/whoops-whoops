using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Info")]
    public int cardIndex;
    public string cardIndicator;

    [Header("Flags")]
    public bool cardClicked;
    public bool cardMatched;

    [Header("Rotation Info")]
    private RectTransform rectTransform;
    private float rotateSpeed = 1f;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public IEnumerator FlipCard(float rotateAngle)
    {
        float time = 0;
        while (time < 1)
        {
            rectTransform.rotation = Quaternion.Slerp(rectTransform.rotation, Quaternion.Euler(0, rotateAngle, 0), time);
            time += Time.deltaTime * rotateSpeed;

            yield return null;
        }
    }
}
