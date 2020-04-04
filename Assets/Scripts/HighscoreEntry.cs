using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreEntry : MonoBehaviour {

	public TextMeshProUGUI m_Text;
	public Image m_Image;

	public void SetData(int place, int time){
		string outputText = $"{place}. ";
		outputText += string.Format("{0}:{1:00}", time / 60, time % 60);
		m_Text.text = outputText;
	}

}
