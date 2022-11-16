using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

public class Timer : MonoBehaviour
{
    public string m_Timer = @"00 : 00";
    private bool m_IsPlaying;
    public float m_TotalSeconds = 5 * 60;
    public Text m_Text;
    // Start is called before the first frame update
    private void Start()
    {
        m_Timer = CountdownTimer(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_IsPlaying = true;

        if (m_IsPlaying)
        {
            m_Timer = CountdownTimer();
        }

        if (m_TotalSeconds <= 0)
        {
            SetZero();
            EditorApplication.isPaused = true;
        }

        if (m_Text) m_Text.text = m_Timer;
    }

    private string CountdownTimer(bool IsUpdate = true)
    {
        if (IsUpdate) m_TotalSeconds -= Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(m_TotalSeconds);
        string timer = string.Format("{0:00} : {1:00}", timeSpan.Minutes, timeSpan.Seconds);

        return timer;
    }

    private void SetZero()
    {
        m_Timer = @"00 : 00";
        m_TotalSeconds = 0;
        m_IsPlaying = false;
    }
}
