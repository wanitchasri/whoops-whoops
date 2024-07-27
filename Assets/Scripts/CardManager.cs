using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Info")]
    public string cardIndicator;

    [Header("Flags")]
    public bool cardClicked;
    public bool cardMatched;

    [Header("Rotation Info")]
    private RectTransform rectTransform;
    private float rotateSpeed = 0.5f;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!cardClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.name == gameObject.name)
                    {
                        cardClicked = true;
                    }
                }
            }
        }
        else
        {
            StartCoroutine(FlipCard(-180f));
        }
    }

    IEnumerator FlipCard(float rotateAngle)
    {
        float time = 0;
        while (time < 1)
        {
            rectTransform.rotation = Quaternion.Slerp(rectTransform.rotation, Quaternion.Euler(0, rotateAngle, 0), time);
            time += Time.deltaTime * rotateSpeed;

            yield return null;
        }
    }

    //set indicator of each card - picture
    // indicator whether this card is matched
    // if the card is not matched, can be clicked again
    // if the card is matched, cannot be clickeed
}
