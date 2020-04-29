using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashBin : MonoBehaviour
{
    public bool m_IsOpen = false;
    public bool m_TransferInProgress = false;
    public BinType m_BinType = BinType.Green;
    public Transform m_LidBone;
    public float m_LidClosedAngle;
    public float m_LidOpenAngle;

    public Transform m_DropOffPoint;

    public void OpenLid()
    {
        if (m_IsOpen == false && GameController.instance.m_GameStarted)
        {
            m_LidBone.DOKill();
            SoundController.instance.PlayOneShot("LidOpen");
            m_LidBone.DOLocalRotateQuaternion(Quaternion.AngleAxis(m_LidOpenAngle, -m_LidBone.right), 0.5f);
            m_IsOpen = true;
        }
    }

    public void CloseLid()
    {
        if (m_IsOpen == true && m_TransferInProgress == false && GameController.instance.m_GameStarted)
        {
            m_LidBone.DOKill();
            SoundController.instance.PlayOneShot("LidClose");
            m_LidBone.DOLocalRotateQuaternion(Quaternion.AngleAxis(m_LidClosedAngle, -m_LidBone.right), 0.5f);
            m_IsOpen = false;
        }
    }
}
