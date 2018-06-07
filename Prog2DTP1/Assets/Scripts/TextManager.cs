using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct CharacterRep
{
    public string m_EventName;
    public string m_Question;
    public int m_Reponse;
    public string m_Choice1;
    public string m_Choice2;
    public string m_Choice3;
    public string m_ReponseEnnemyBon;
    public string m_ReponseEnnemyMauvais;
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
#region EditorVariables
    [SerializeField]
    private float m_TexteDelay = 1f;
    [SerializeField]
    private float m_EnnemiReponseTime = 5f;
    [SerializeField]
    private TextMeshProUGUI m_EnnemiTextBox;
    [SerializeField]
    private PlayerChoicesButton PlayerChoicesButton;
    [SerializeField]
    private List<CharacterRep> m_Textes = new List<CharacterRep>();
    [SerializeField]
    private Animator m_PlayerAnimator;
    [SerializeField]
    private Animator m_EnnemyAnimator;
    [SerializeField]
    private Transform m_PlayerTransform;
    [SerializeField]
    private Transform m_EnnemyTransform;
    [SerializeField]
    private AudioClip m_ShootSound;
    [SerializeField]
    private AudioClip m_TextSound;
    [SerializeField]
    private AudioSource m_AudioSource;
    #endregion

#region PrivatesVariables
    private bool m_WaitingPlayerAction = false;
    private bool m_WaitingEnnemySpeech = false;
    private int m_RenduDansHistoire = 0;
    private int m_Decision = 0;
    private int m_RenduDansString = 0;
    private int m_ErreurCompte = 0;
    private float m_TimeCounter = 0;
    private float m_EnnemyTimeCounter = 0;
    private string m_CurrentString;
    private char[] m_ParsedString;
#endregion

    private void Start ()
    {
        m_TimeCounter = m_TexteDelay;
        m_ParsedString = PlayerDecision(m_Textes[0].m_Question);
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
        else if (!m_WaitingPlayerAction && m_RenduDansString >= m_ParsedString.Length && m_WaitingEnnemySpeech && m_EnnemiReponseTime > m_EnnemyTimeCounter)
        {
            m_EnnemyTimeCounter += Time.deltaTime;
            m_AudioSource.Stop();
        }
        else if (!m_WaitingPlayerAction && m_RenduDansString >= m_ParsedString.Length && m_WaitingEnnemySpeech && m_EnnemiReponseTime > m_EnnemyTimeCounter && Input.GetMouseButtonDown(0))
        {
            m_EnnemyTimeCounter += m_EnnemiReponseTime;
            m_AudioSource.Stop();
        }
        else if (!m_WaitingPlayerAction && m_RenduDansString >= m_ParsedString.Length && m_WaitingEnnemySpeech && m_EnnemiReponseTime <= m_EnnemyTimeCounter)
        {
            m_EnnemyTimeCounter = 0;
            m_WaitingEnnemySpeech = false;
            CleanBetwenChoices();
            m_ParsedString = PlayerDecision(m_Textes[m_RenduDansHistoire].m_Question);
        }
        else if (!m_WaitingPlayerAction && m_RenduDansString >= m_ParsedString.Length && !m_WaitingEnnemySpeech)
        {
            ActivatePlayerChoices();
            m_WaitingPlayerAction = true;
            m_AudioSource.Stop();
            m_EnnemyTimeCounter = 0;
        }

    }

    public void PlayerChose(int i_Choice)
    {
        m_RenduDansHistoire++;
        m_WaitingEnnemySpeech = true;
        if (m_RenduDansHistoire  == m_Textes.Count - 1)
        {
            switch (i_Choice)
            {
                case 0:
                    m_PlayerAnimator.SetTrigger("Shoot");
                    m_AudioSource.PlayOneShot(m_ShootSound, 0.7F);
                    m_PlayerAnimator.SetTrigger("JustShoot");
                    m_EnnemyAnimator.SetTrigger("JustDie");
                    break;
                case 1:
                    m_EnnemyAnimator.SetTrigger("Die");
                    m_PlayerAnimator.SetTrigger("Die");
                    m_PlayerAnimator.SetTrigger("Shoot");
                    m_EnnemyAnimator.SetTrigger("Shoot");
                    m_AudioSource.PlayOneShot(m_ShootSound, 0.7F);
                    
                    break;
                case 2:
                    m_EnnemyAnimator.SetTrigger("Shoot");
                    m_AudioSource.PlayOneShot(m_ShootSound, 0.7F);
                    m_EnnemyAnimator.SetTrigger("JustShoot");
                    m_PlayerAnimator.SetTrigger("JustDie");
                    break;
                default:
                    break;
            }
        }
        if (m_RenduDansHistoire  >= m_Textes.Count)
        {
            switch (i_Choice)
            {
                case 0:
                    m_RenduDansHistoire = 0;
                    CleanBetwenChoices();
                    m_ParsedString = PlayerDecision(m_Textes[0].m_Question);
                    m_EnnemyAnimator.SetTrigger("Restart");
                    m_PlayerAnimator.SetTrigger("Restart");
                    m_ErreurCompte = 0;
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
        else if(i_Choice + 1 != m_Textes[m_RenduDansHistoire - 1].m_Reponse)
        {
            m_ErreurCompte++;
            if (m_ErreurCompte >= 3)
            {
                m_EnnemyAnimator.SetTrigger("Shoot");
                m_AudioSource.PlayOneShot(m_ShootSound, 0.7F);
                m_EnnemyAnimator.SetTrigger("JustShoot");
                m_PlayerAnimator.SetTrigger("JustDie");
                CleanBetwenChoices();
                m_RenduDansHistoire = m_Textes.Count-1;
            }
            m_ParsedString = PlayerDecision(m_Textes[m_RenduDansHistoire - 1].m_ReponseEnnemyMauvais);

        }
        else
        {
            m_ParsedString = PlayerDecision(m_Textes[m_RenduDansHistoire - 1].m_ReponseEnnemyBon);
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
        if (m_Textes[m_RenduDansHistoire].m_Choice1 != "NA")
        {
            PlayerChoicesButton.m_PlayerChoicesButton1.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText1.SetText(m_Textes[m_RenduDansHistoire].m_Choice1);
        }
        if (m_Textes[m_RenduDansHistoire].m_Choice2 != "NA")
        {
            PlayerChoicesButton.m_PlayerChoicesButton2.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText2.SetText(m_Textes[m_RenduDansHistoire].m_Choice2);
        }
        if (m_Textes[m_RenduDansHistoire].m_Choice3 != "NA")
        {
            PlayerChoicesButton.m_PlayerChoicesButton3.SetActive(true);
            PlayerChoicesButton.m_PlayerChoicesText3.SetText(m_Textes[m_RenduDansHistoire].m_Choice3);
        }
    }

    private char[] PlayerDecision(string i_Texte)
    {
        return i_Texte.ToCharArray();
    }

    private void DisplayTexte(bool i_Instant = false)
    {
        m_AudioSource.PlayOneShot(m_TextSound, 1F);
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
