using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject intro;
    public GameObject gamemenu;
    public GameObject flashbang;

    void Start()
    {
        flashbang.SetActive(false);
        StartCoroutine(Flash());
        video.loopPointReached += EndReached;
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.8f);
        flashbang.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flashbang.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        intro.SetActive(false);
        gamemenu.SetActive(true);
    }
}