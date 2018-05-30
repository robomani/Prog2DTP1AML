using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct CharacterRep
{
    public string m_EventName;
    public string m_Choice1;
    public string m_Choice2;
    public string m_Choice3;
}

[System.Serializable]
public struct PlayerChoicesButton
{
    public GameObject m_PlayerChoicesButton1;
    public TextMeshProUGUI m_PlayerChoicesText1;
    public GameObject m_PlayerChoicesButton2;
    public TextMeshProUGUI m_PlayerChoicesText2;
    public GameObject m_PlayerChoicesButton3;
    public TextMeshProUGUI m_PlayerChoicesText3;
}

public class TextManager : MonoBehaviour
{

    [SerializeField]
    private int m_LenghtOfStory = 3;
    [SerializeField]
    private float m_TexteDelay = 1f;
    [SerializeField]
    private TextMeshProUGUI m_EnnemiTextBox;
    [SerializeField]
    private PlayerChoicesButton PlayerChoicesButton;
    [SerializeField]
    private List<CharacterRep> m_TextesEnnemi = new List<CharacterRep>();
    [SerializeField]
    private List<CharacterRep> m_TextesPlayer = new List<CharacterRep>();

    private bool m_WaitingPlayerAction = false;
    private int m_RenduDansHistoire = 0;
    private int m_Decision = 0;
    private int m_RenduDansString = 0;
    private float m_TimeCounter = 0;
    private string m_CurrentString;
    private char[] m_ParsedString;

    private void Start ()
    {
        m_TimeCounter = m_TexteDelay;
        m_ParsedString = PlayerDecision(m_TextesEnnemi[0].m_Choice1);
    }


    private void Update ()
    {
        m_TimeCounter += Time.deltaTime;
        
        if (!m_WaitingPlayerAction && m_TexteDelay < m_TimeCounter && m_RenduDansString < m_ParsedString.Length)
        {
            DisplayTexte();
        }
        else if (!m_WaitingPlayerAction && m_RenduDansString < m_ParsedString.Length && Input.GetMouseButtonDown(0))
        {
            DisplayTexte(true);
        }
        else if (!m_WaitingPlayerAction && m_RenduDansString >= m_ParsedString.Length)
        {
            ActivatePlayerChoices();
            m_WaitingPlayerAction = true;
        }

    }

    public void PlayerChose(int i_Choice)
    {
        m_RenduDansHistoire++;
        if (m_RenduDansHistoire >= m_LenghtOfStory)
        {
            switch (i_Choice)
            {
                case 0:
                    m_RenduDansHistoire = 0;
                    m_ParsedString = PlayerDecision(m_TextesEnnemi[0].m_Choice1);
                    break;
                case 1:
                    Application.Quit();
                    break;
                case 2:

                    break;
                default:
                    break;
            }
        }
        else if (m_RenduDansHistoire == m_LenghtOfStory - 1)
        {
            switch (i_Choice)
            {
                case 0:
                    m_RenduDansHistoire = 0;
                    m_ParsedString = PlayerDecision(m_TextesEnnemi[0].m_Choice1);
                    break;
                case 1:
                    Application.Quit();
                    break;
                case 2:

                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (i_Choice)
            {   
                case 0:
                    m_ParsedString = PlayerDecision(m_TextesEnnemi[m_RenduDansHistoire].m_Choice1);
                    break;
                case 1:
                    m_ParsedString = PlayerDecision(m_TextesEnnemi[m_RenduDansHistoire].m_Choice2);
                    break;
                case 2:
                    m_ParsedString = PlayerDecision(m_TextesEnnemi[m_RenduDansHistoire].m_Choice3);
                    break;
                default:
                    break;
            }
            m_Decision = i_Choice;
        }     
        CleanBetwenChoices();
    }

    private void CleanBetwenChoices()
    {
        m_RenduDansString = 0;
        m_CurrentString = "";
        PlayerChoicesButton.m_PlayerChoicesButton1.SetActive(false);
        PlayerChoicesButton.m_PlayerChoicesButton2.SetActive(false);
        PlayerChoicesButton.m_PlayerChoicesButton3.SetActive(false);
        m_WaitingPlayerAction = false;
    }

    private void ActivatePlayerChoices()
    {
            PlayerChoicesButton.m_PlayerChoicesButton1.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText1.SetText(m_TextesPlayer[m_RenduDansHistoire].m_Choice1);
            PlayerChoicesButton.m_PlayerChoicesButton2.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText2.SetText(m_TextesPlayer[m_RenduDansHistoire].m_Choice2);
            PlayerChoicesButton.m_PlayerChoicesButton3.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText3.SetText(m_TextesPlayer[m_RenduDansHistoire].m_Choice3);
    }

    private char[] PlayerDecision(string i_Texte)
    {
        return i_Texte.ToCharArray();
    }

    private void DisplayTexte(bool i_Instant = false)
    {
        if (!i_Instant)
        {
            m_CurrentString += m_ParsedString[m_RenduDansString];
            m_TimeCounter = 0;
            m_RenduDansString++;
        }
        else
        {
            m_CurrentString = "";
            for (int i = 0; i < m_ParsedString.Length; i++)
            {
                m_CurrentString += m_ParsedString[i];
            }
            m_RenduDansString = m_ParsedString.Length;        
        }
        m_EnnemiTextBox.SetText(m_CurrentString);
    }
}
