using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenRotatePingPong : MonoBehaviour
{
    Vector3 m_Rotation = new Vector3(0f, 0f, -20f);

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(m_Rotation, 2f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
