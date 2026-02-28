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
        // Make the intro flash
        yield return new WaitForSeconds(0.8f);
        flashbang.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flashbang.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        // When video end, we display the menu
        intro.SetActive(false);
        gamemenu.SetActive(true);
    }
}