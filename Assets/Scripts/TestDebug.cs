using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestDebug : MonoBehaviour
{
    public Camera m_Cam;
    public TextMeshProUGUI m_TextOutput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_TextOutput.text = m_Cam.fieldOfView.ToString();
    }
}
