using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour {

	public UnityEvent m_ClickEvent;

	private Image m_ButtonImage; 

	public Color m_NormalColor = Color.white;
	public Color m_HoverColor = Color.white;
	public Color m_PressedColor = Color.white;

	public enum ButtonState { None, Normal, Hover, Pressed}
	public ButtonState m_CurrentButtonState;
	private ButtonState m_LastButtonState;

	public void LoadScene(string _sceneName)
	{
		SceneLoadController.instance.LoadScene(_sceneName);
	}

	public void StartGame()
	{
		GameController.instance.StartGame();
	}
	public void StopGame()
	{
		GameController.instance.StopGame();
	}

	void Start()
	{
		m_ButtonImage = GetComponent<Image>();
		SetButtonState(ButtonState.Normal);
	}

	void Update()
	{
		UpdateButtonState();
	}

	public void SetButtonState(ButtonState _state)
	{
		m_CurrentButtonState = _state;
	}

	public void UpdateButtonState()
	{
        if (m_CurrentButtonState != m_LastButtonState)
        {
            switch (m_CurrentButtonState)
            {
                case ButtonState.Normal:
					m_ButtonImage.DOColor(m_NormalColor, 0.15f);
					m_LastButtonState = m_CurrentButtonState;
                    return;
                case ButtonState.Hover:
					m_ButtonImage.DOColor(m_HoverColor, 0.15f);
					m_LastButtonState = m_CurrentButtonState;
                    return;
                case ButtonState.Pressed:
					m_ButtonImage.DOColor(m_PressedColor, 0.15f);
					m_ClickEvent.Invoke();
					m_LastButtonState = m_CurrentButtonState;
                    return;
                default:
                    return;
            }
        }
    }

}