using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreController : MonoBehaviour
{
    public int m_Score = 0;

    public float m_Timer = 0f;
    public int m_Seconds = 0;

    public int[] m_Highscores;
    public CanvasGroup m_HighscoresCanvas;
    public Transform m_HighscoreEntryContainer;
    public HighscoreEntry m_HighscoreEntryPrefab;
    public List<HighscoreEntry> m_HighscoreEntries;
    public Color m_HighscoreBaseColor;
    public Color m_HighscorePlayerColor;
    public TextMeshProUGUI m_PlayerTime;

    public CanvasGroup m_ScoreContainer;
    public TextMeshProUGUI m_ScoreOutput;

    private int m_MaxScore;

    private bool m_FirstStart = true;

    public static GameScoreController instance;

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
        m_FirstStart = true;
        Initialize();
    }
    public void Initialize()
    {
        m_HighscoresCanvas.alpha = 0f;
        m_HighscoresCanvas.gameObject.SetActive(false);
        CalculateMaxScore();
        m_Score = 0;
        m_Timer = 0f;
        m_Seconds = 0;
        if (m_FirstStart == false)
        {
            GameController.instance.m_GameStarted = true;
        }
        m_Highscores = PlayerPrefsX.GetIntArray("Highscore", 600, 10);
        //CleanHighscoreTable();
        m_ScoreContainer.alpha = 1f;
        UpdateScoreUI();
        m_FirstStart = false;
    }

    void CleanHighscoreTable()
    {
        foreach(Transform t in m_HighscoreEntryContainer)
        {
            Destroy(t.gameObject);
        }
    }

    public void ResetHighscores()
    {
        m_Highscores = PlayerPrefsX.GetIntArray("Highscore", 600, 10);
        for (int i = 0; i < m_Highscores.Length; i++)
        {

            m_Highscores[i] = 600;
            PlayerPrefsX.SetIntArray("Highscore", m_Highscores);
        }
    }

    void CalculateMaxScore()
    {
        InteractableObject[] allInteractables = GameObject.FindObjectsOfType<InteractableObject>();
        m_MaxScore = allInteractables.Length;
    }

    void Update()
    {
        if(GameController.instance.m_GameStarted == true)
        {
            m_Timer += Time.deltaTime;
            m_Seconds = Mathf.RoundToInt(m_Timer);
        }
    }

    public void AddToScore()
    {
        m_Score++;
        UpdateScoreUI();
        CheckForGameEnd();
    }

    void UpdateScoreUI()
    {
        m_ScoreOutput.text = ($"{m_Score} / {m_MaxScore}");
    }

    void CheckForGameEnd()
    {
        if (m_Score >= m_MaxScore)
        {
            StartCoroutine( DoGameEnd());
        }
    }

    IEnumerator DoGameEnd()
    {
        GameController.instance.m_GameStarted = false;
        m_PlayerTime.text = string.Format("{0}:{1:00}", m_Seconds / 60, m_Seconds % 60);

        m_ScoreContainer.alpha = 0f;

        int maxVal = m_Highscores.Max();
        int maxID = m_Highscores.ToList().IndexOf(maxVal);
        if (m_Seconds <= maxVal || maxVal == 0)
        {
            if (!IsElementPresent(m_Seconds))
                m_Highscores[maxID] = m_Seconds;
        }

        Array.Sort(m_Highscores);
        m_HighscoresCanvas.gameObject.SetActive(true);
        m_HighscoresCanvas.DOFade(1f, 0.25f);
        yield return new WaitForSeconds(0.25f);

        if (m_HighscoreEntryContainer.childCount > 0)   // if there are already HS entries instantiated then reuse them
        {
            for(int i = 0; i < m_HighscoreEntries.Count; i++)
            {
                SetUpHighscoreEntry(m_HighscoreEntries[i], i);

                yield return new WaitForSeconds(0.05f);
            }
        }
        else  // if there no HS entries instantiated then create new ones
        {
            for (int i = 0; i < m_Highscores.Length; i++)
            {
                HighscoreEntry hsEntry = Instantiate(m_HighscoreEntryPrefab, m_HighscoreEntryContainer);
                SetUpHighscoreEntry(hsEntry, i);
                m_HighscoreEntries.Add(hsEntry);

                yield return new WaitForSeconds(0.05f);
            }
        }
        PlayerPrefsX.SetIntArray("Highscore", m_Highscores);
    }

    void SetUpHighscoreEntry(HighscoreEntry _entryToUpdate, int _id)
    {
        _entryToUpdate.transform.localScale = Vector3.zero;
        if (m_Highscores[_id] == m_Seconds)
        {
            _entryToUpdate.m_Image.color = m_HighscorePlayerColor;
        }
        else
        {
            _entryToUpdate.m_Image.color = m_HighscoreBaseColor;
        }

        _entryToUpdate.transform.DOScale(1f, 0.25f);
        _entryToUpdate.SetData(_id + 1, m_Highscores[_id]);
    }

    bool IsElementPresent(int element)
    {
        for (int i = 0; i < m_Highscores.Length; i++)
        {
            if (element == m_Highscores[i])
            {
                return true;
            }
        }
        return false;
    }
}
