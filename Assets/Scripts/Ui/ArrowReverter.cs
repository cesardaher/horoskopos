using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReverter : MonoBehaviour
{

    [SerializeField] DataPanelOpener _dataPanelOpener;

    bool _pointingDown;

    void Start()
    {
        _pointingDown = false;
    }

    void Update()
    {
        if(_dataPanelOpener.Opened && !_pointingDown)
            PointArrowDown();

        if(!_dataPanelOpener.Opened && _pointingDown)
            PointArrowUp();
    }

    void PointArrowDown()
    {
        var scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;

        _pointingDown = true;
    }

    void PointArrowUp()
    {
        var scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;

        _pointingDown = false;
    }
}
