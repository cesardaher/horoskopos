using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sign2D : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public int signID;
    public void TargetSignWithCamera()
    {
        EventManager.Instance.InteractWith2DSign(signID);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        TargetSignWithCamera();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
