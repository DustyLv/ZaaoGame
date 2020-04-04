using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public bool m_GameStarted = false;

    public CanvasGroup m_GameUI;
    public CanvasGroup m_MainMenuUI;

    public InteractableObject[] m_InteractableObjects;

    public delegate void ActionDelegate();
    public ActionDelegate aDelegate;

    private bool m_StartGameUIToggleCompleted = false;
    private bool m_StopGameUIToggleCompleted = false;

    public static GameController instance;

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
        m_InteractableObjects = GameObject.FindObjectsOfType<InteractableObject>();

        StopGame();
    }

    public void StartGame()
    {
        ResetScene();
        ToggleMainMenuUI(false);
        ToggleGameUI(true);
    }

    public void StopGame()
    {
        ToggleGameUI(false);
        ToggleMainMenuUI(true);
    }

    public void ToggleGameUI(bool _state)
    {
        float stateValue = _state == true ? 1f : 0f;

        m_GameUI.DOFade(stateValue, 0.5f).OnStart(() =>
        {
            if (_state == true)
            {
                m_GameUI.gameObject.SetActive(_state);
                GameScoreController.instance.Initialize();
                ActionController.instance.Initialize();
            }
        }).OnComplete(() =>
        {
            if (_state == false)
            {
                m_GameUI.gameObject.SetActive(_state);
            }
        });
    }

    public void ToggleMainMenuUI(bool _state)
    {
        float stateValue = _state == true ? 1f : 0f;

        m_MainMenuUI.DOFade(stateValue, 0.5f).OnStart(() =>
        {
            if (_state == true)
            {
                m_MainMenuUI.gameObject.SetActive(_state);
            }
        }).OnComplete(() =>
        {
            if (_state == false)
            {
                m_MainMenuUI.gameObject.SetActive(_state);
            }
        });
    }

    public void ResetScene()
    {
        foreach(InteractableObject obj in m_InteractableObjects)
        {
            obj.ResetObject();
        }
    }
}
