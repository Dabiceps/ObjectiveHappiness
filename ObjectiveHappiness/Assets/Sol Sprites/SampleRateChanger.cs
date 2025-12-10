using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleRateChanger : MonoBehaviour
{
    [Header("Terrain")]
    public GameObject terrain;

    [Header("Buttons")]
    public Button pause;
    public Button resume;
    public Button x2;
    public Button x3;

    private void Start()
    {
        pause.onClick.AddListener(() => OnPauseClick());
        resume.onClick.AddListener(() => OnResumeClick());
        x2.onClick.AddListener(() => OnX2Click());
        x3.onClick.AddListener(() => OnX3Click());
    }

    void OnPauseClick()
    {
        SetSpeed(0f);
    }
    void OnResumeClick()
    {
        SetSpeed(1f);
    }
    void OnX2Click()
    {
        SetSpeed(2f);
    }

    void OnX3Click()
    {
        SetSpeed(3f);
    }

    void SetSpeed(float speed)
    {
        if (terrain == null)
        {
            Debug.LogWarning("Terrain non assigné !");
            return;
        }

        Animator[] animators = terrain.GetComponentsInChildren<Animator>(true);
        if (animators == null || animators.Length == 0)
        {
            Debug.LogWarning("Aucun Animator trouvé sur le terrain !");
            return;
        }


        foreach (Animator animator in animators)
        {
            if (animator != null)
                animator.speed = speed;
        }
    }


}
