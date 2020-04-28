using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleCanvasGroup : MonoBehaviour
{
    public bool m_StartHidden = true;

    private bool m_State = false;
    private CanvasGroup m_CanvasGroup;

    void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();

        if (m_StartHidden)
        {
            m_CanvasGroup.DOFade(0f, 0.1f);
            m_State = false;
        }
        else
        {
            m_CanvasGroup.DOFade(1f, 0.1f);
            m_State = true;
        }
    }

    public void ToggleCanvas()
    {
        m_State = !m_State;
        float stateValue = m_State == true ? 1f : 0f;
        m_CanvasGroup.DOFade(stateValue, 0.1f);
    }
}
