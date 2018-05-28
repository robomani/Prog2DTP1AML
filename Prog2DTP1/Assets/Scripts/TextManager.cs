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
    [SerializeField]
    private float m_TexteDelay = 1f;

    private int m_Decision = 0;
    private int m_RenduDansString = 0;
    private string m_CurrentString;
    private float m_timeCounter = 0;
    private char[] m_ParsedString;

    void Start ()
    {
        m_timeCounter = m_TexteDelay;
        m_ParsedString = PlayerDecision(m_Intro);
    }
	

	void Update ()
    {
        m_timeCounter += Time.deltaTime;
        if (m_TexteDelay < m_timeCounter && m_RenduDansString < m_ParsedString.Length)
        {
            m_CurrentString += m_ParsedString[m_RenduDansString];
            m_timeCounter = 0;
            m_RenduDansString++;
            m_PlayerTextBox.text = m_CurrentString;
        }
        else if (m_RenduDansString < m_ParsedString.Length)
        {

        }

    }

    public char[] PlayerDecision(string i_Texte)
    {
        return i_Texte.ToCharArray();
    }

    public void DisplayTexte()
    {

    }
}
