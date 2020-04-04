using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionController : MonoBehaviour
{
    public Transform m_RaycastOrigin;
    //private bool m_HasGameInteractable = false;

    public LayerMask m_InteractableLayerMask = -1;
    public LayerMask m_TrashBinLayerMask = -1;
    public LayerMask m_UIInteractableLayerMask = -1;

    public Image m_Reticle;
    public Color m_NormalColor = Color.white;
    public Color m_HoveringColor = Color.yellow;
    public Color m_PickedUpColor = Color.green;

    public CanvasGroup m_WrongBinImage;

    private InteractableObject m_CurrentInteractableObject;
    public Transform m_PickupPoint;

    private bool m_UserButtonPressed = false;

    public enum PickupState { Normal, Hovering, PickedUp}
    private PickupState m_CurrentPickupState = PickupState.Normal;
    private PickupState m_LastPickupState;

    private TrashBin m_CurrentTrashBin;
    private ButtonVR m_CurrentButton;


    public static ActionController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_WrongBinImage.alpha = 0f;
    }

    void FixedUpdate()
    {
        RaycastHit hitInteractable;
        if (Physics.Raycast(m_RaycastOrigin.position, m_RaycastOrigin.TransformDirection(Vector3.forward), out hitInteractable, Mathf.Infinity, m_InteractableLayerMask))
        {
            m_CurrentPickupState = PickupState.Hovering;

            if (m_UserButtonPressed)
            {
                if (m_CurrentInteractableObject == null) { m_CurrentInteractableObject = hitInteractable.collider.GetComponent<InteractableObject>(); }
            }
        }
        else
        {
            m_CurrentPickupState = PickupState.Normal;
        }

        RaycastHit hitUI;
        if (Physics.Raycast(m_RaycastOrigin.position, m_RaycastOrigin.TransformDirection(Vector3.forward), out hitUI, Mathf.Infinity, m_UIInteractableLayerMask))
        {
            m_CurrentPickupState = PickupState.Hovering;

            if (m_CurrentButton == null)
            {
                m_CurrentButton = hitUI.collider.GetComponent<ButtonVR>();
            }
            if(m_CurrentButton != null)
            {
                m_CurrentButton.SetButtonState(ButtonVR.ButtonState.Hover);
            }

            if (m_UserButtonPressed && m_CurrentInteractableObject == null)
            {
                if (m_CurrentButton != null)
                {
                    m_CurrentButton.SetButtonState(ButtonVR.ButtonState.Pressed);
                    //m_CurrentButton.m_ClickEvent.Invoke();
                }
            }
        }
        else
        {
            if (m_CurrentButton != null)
            {
                m_CurrentButton.SetButtonState(ButtonVR.ButtonState.Normal);
                m_CurrentButton = null;

                m_CurrentPickupState = PickupState.Normal;
            }
        }

        RaycastHit hitTrashBin;
        if (Physics.Raycast(m_RaycastOrigin.position, m_RaycastOrigin.TransformDirection(Vector3.forward), out hitTrashBin, Mathf.Infinity, m_TrashBinLayerMask))
        {
            if(m_CurrentTrashBin == null)
            {
                m_CurrentTrashBin = hitTrashBin.collider.GetComponent<TrashBin>();
            }
            m_CurrentTrashBin.OpenLid();
        }
        else
        {
            if(m_CurrentTrashBin != null)
            {
                m_CurrentTrashBin.CloseLid();
                m_CurrentTrashBin = null;
            }
        }

        if (!m_UserButtonPressed)
        {
            m_PickupPoint.localPosition = new Vector3(0f, 0f, hitInteractable.distance);
        }

    }

    void DropCurrentObject()
    {
        if(m_CurrentInteractableObject != null && m_CurrentTrashBin != null)
        {
            if(m_CurrentInteractableObject.m_TargetBin == m_CurrentTrashBin.m_BinType)
            {
                //m_CurrentTrashBin.m_CurrentTrashState = TrashBin.TrashState.Transferring;
                m_CurrentInteractableObject.Drop(m_CurrentTrashBin);
            }
            else
            {
                // return object to starting position
                m_CurrentInteractableObject.ReturnToStartPosition();
                // show red cross
                m_WrongBinImage.DOKill();
                m_WrongBinImage.DOFade(1f, 0.15f).OnStart(() =>
                {
                    m_WrongBinImage.transform.localScale = Vector3.zero;
                    m_WrongBinImage.transform.DOScale(Vector3.one * 1.5f, 0.75f).SetEase(Ease.OutBack);
                }).OnComplete(() =>
                {
                    m_WrongBinImage.transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(180f, Vector3.forward), 0.8f).OnComplete(() =>
                    {
                        m_WrongBinImage.transform.localRotation = Quaternion.identity;
                        m_WrongBinImage.transform.DOScale(Vector3.zero, 0.3f).SetDelay(1f).SetEase(Ease.InBack).OnComplete(() =>
                        {
                            m_WrongBinImage.DOFade(0f, 0.1f);
                        });
                    });
                });
            }
            
            m_CurrentInteractableObject = null;
            m_CurrentPickupState = PickupState.Normal;
        }
        else
        {
            if (m_CurrentInteractableObject != null)
            {
                m_CurrentInteractableObject.ReturnToStartPosition();
                m_CurrentInteractableObject = null;
            }
        }
    }

    void Update()
    {
        UpdatePickupState();

        if (Input.GetButtonDown("Tap"))
        {
            m_UserButtonPressed = true;
        }
        if (Input.GetButtonUp("Tap"))
        {
            m_UserButtonPressed = false;
        }

        if (m_UserButtonPressed)
        {
            if (m_CurrentInteractableObject != null)
            {
                m_CurrentPickupState = PickupState.PickedUp;
                
            }
            //ButtonVR btn = hit.collider.GetComponent<ButtonVR>();
            //if (btn != null) btn.OnSelected();
        }
        else
        {
            DropCurrentObject();
        }
    }

    void UpdatePickupState()
    {
        if(m_CurrentPickupState != m_LastPickupState)
        {
            switch (m_CurrentPickupState)
            {
                case PickupState.Normal:
                    m_Reticle.DOColor(m_NormalColor, 0.2f);
                    m_Reticle.rectTransform.DOScale(1f, 0.2f);
                    m_LastPickupState = m_CurrentPickupState;
                    return;
                case PickupState.Hovering:
                    m_Reticle.DOColor(m_HoveringColor, 0.2f);
                    m_Reticle.rectTransform.DOScale(1.4f, 0.2f);
                    m_LastPickupState = m_CurrentPickupState;
                    return;
                case PickupState.PickedUp:
                    m_Reticle.DOColor(m_PickedUpColor, 0.2f);
                    m_Reticle.rectTransform.DOScale(0.2f, 0.2f);
                    m_CurrentInteractableObject.PickUp(m_PickupPoint);
                    m_LastPickupState = m_CurrentPickupState;
                    return;
                default:
                    return;
            }
        }
    }
}
