using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    RectTransform rectTransform; 
    // float r;
    bool cardClicked;
    float rotateSpeed = 1f;

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
            StartCoroutine(RotateCard(-180f));
        }
    }

    IEnumerator RotateCard(float rotateAngle)
    {
        float time = 0;
        while (time < 1)
        {
            rectTransform.rotation = Quaternion.Slerp(rectTransform.rotation, Quaternion.Euler(0, rotateAngle, 0), Time.deltaTime * 1f);
            time += Time.deltaTime * rotateSpeed;

            yield return null;

        }
    }

}
