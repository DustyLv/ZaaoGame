using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[RequireComponent(typeof(Rigidbody))]
public class InteractableObject : MonoBehaviour
{
    private bool m_IsPickedUp = false;
    private Transform m_Parent;
    private Vector3 m_InitialPosition;
    private Quaternion m_InitialRotation;
    private Vector3 m_InitialScale;

    public BinType m_TargetBin = BinType.Green;
    public float m_AnimationLength = 1f;

    void Start()
    {
        m_InitialPosition = transform.position;
        m_InitialRotation = transform.rotation;
        m_InitialScale = transform.localScale;
    }

    void Update()
    {
        if(m_IsPickedUp && m_Parent != null)
        {
            transform.DOMove(m_Parent.position, 0.1f);
        }
    }

    public void PickUp(Transform _pickupPoint)
    {
        m_IsPickedUp = true;
        m_Parent = _pickupPoint;
    }

    public void Drop(TrashBin _targetBin)
    {
        transform.DOMove(_targetBin.m_DropOffPoint.position, m_AnimationLength).OnStart(() =>
        {
            _targetBin.m_TransferInProgress = true;
            //transform.DORotateQuaternion(Random.rotationUniform, m_AnimationLength);
            m_IsPickedUp = false;
            m_Parent = null;
        }).OnComplete(() =>
        {
            transform.DOBlendableLocalMoveBy(-Vector3.up, m_AnimationLength * 0.5f).OnStart(()=> 
            {
                transform.DOScale(0f, 0.5f);
            }).OnComplete(() =>
            {
                _targetBin.m_TransferInProgress = false;
                _targetBin.CloseLid();
                GameScoreController.instance.AddToScore();
                gameObject.SetActive(false);
            });
        });
    }

    public void ReturnToStartPosition()
    {
        m_IsPickedUp = false;
        m_Parent = null;
        transform.DOMove(m_InitialPosition, m_AnimationLength);
    }

    public void ResetObject()
    {
        gameObject.SetActive(true);
        transform.position = m_InitialPosition;
        transform.rotation = m_InitialRotation;
        transform.localScale = m_InitialScale;
    }
}
