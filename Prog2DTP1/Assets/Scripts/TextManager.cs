using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private string m_Intro;
    [SerializeField]
    private TextMeshProUGUI m_PlayerTextBox;
	
	void Start ()
    {
        m_PlayerTextBox.text = m_Intro;
	}
	

	void Update ()
    {
		
	}
}
