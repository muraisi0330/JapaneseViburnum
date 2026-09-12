using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordController : MonoBehaviour
{
    public string[][] keywords;
    public bool[] hasRecognized;
    public KeywordRecognizer[] m_Recognizer;

    public bool ConsoleKeyword;

    // 外部からキーワードを設定する
    public void Initialize(string[][] keywords, bool consoleKeyword)
    {
        this.keywords = keywords;
        this.ConsoleKeyword = consoleKeyword;
    }

    public void SetKeywords()
    {
        if (keywords == null || keywords.Length == 0)
        {
            Debug.LogError("KeywordController: keywordsが設定されていません。");
            return;
        }

        hasRecognized = new bool[keywords.Length];
        m_Recognizer = new KeywordRecognizer[keywords.Length];

        for (int i = 0; i < keywords.Length; i++)
        {
            SetRecognizer(i);
        }
    }

    private void SetRecognizer(int i)
    {
        if (keywords[i] == null || keywords[i].Length == 0)
        {
            Debug.LogWarning($"keywords[{i}]が空です。");
            return;
        }

        m_Recognizer[i] = new KeywordRecognizer(keywords[i]);

        m_Recognizer[i].OnPhraseRecognized += SetKeywordsBool;

        if (ConsoleKeyword)
        {
            m_Recognizer[i].OnPhraseRecognized += OnPhraseRecognized;
        }
    }

    private void SetKeywordsBool(PhraseRecognizedEventArgs args)
    {
        for (int i = 0; i < keywords.Length; i++)
        {
            if (keywords[i] == null)
                continue;

            for (int j = 0; j < keywords[i].Length; j++)
            {
                if (args.text == keywords[i][j])
                {
                    hasRecognized[i] = true;
                }
            }
        }
    }

    public void StartRecognizing(int i)
    {
        if (m_Recognizer == null)
        {
            Debug.LogError("KeywordRecognizerが初期化されていません。");
            return;
        }

        if (i < 0 || i >= m_Recognizer.Length)
        {
            Debug.LogError(
                $"StartRecognizing: i={i}, Length={m_Recognizer.Length}"
            );
            return;
        }

        if (m_Recognizer[i] == null)
        {
            Debug.LogError($"m_Recognizer[{i}]がnullです。");
            return;
        }

        if (!m_Recognizer[i].IsRunning)
        {
            m_Recognizer[i].Start();
        }
    }

    public void StopRecognizing(int i)
    {
        if (m_Recognizer == null)
            return;

        if (i < 0 || i >= m_Recognizer.Length)
            return;

        if (m_Recognizer[i] == null)
            return;

        if (m_Recognizer[i].IsRunning)
        {
            m_Recognizer[i].Stop();
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendFormat(
            "{0} ({1}){2}",
            args.text,
            args.confidence,
            Environment.NewLine
        );

        builder.AppendFormat(
            "\tTimestamp: {0}{1}",
            args.phraseStartTime,
            Environment.NewLine
        );

        builder.AppendFormat(
            "\tDuration: {0} seconds{1}",
            args.phraseDuration.TotalSeconds,
            Environment.NewLine
        );

        Debug.Log(builder.ToString());
    }

    private void OnDestroy()
    {
        if (m_Recognizer == null)
            return;

        for (int i = 0; i < m_Recognizer.Length; i++)
        {
            if (m_Recognizer[i] != null)
            {
                if (m_Recognizer[i].IsRunning)
                    m_Recognizer[i].Stop();

                m_Recognizer[i].Dispose();
            }
        }
    }
}