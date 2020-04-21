using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class IntroCutsceneSequence : MonoBehaviour
{
    public GameObject gameCam;
    public GameObject cam1;
    public GameObject cam2;

    void Start()
    {
        // start cutscene
        StartCoroutine(TheSequence());
    }

    IEnumerator TheSequence()
    {
        yield return new WaitForSeconds(3.33f);
        cam2.SetActive(true);
        cam1.SetActive(false);
        yield return new WaitForSeconds(2.933f);
        gameCam.SetActive(true);
        cam2.SetActive(false);
        // enable player movement after cutscene ends
        GameObject.Find("Player Ship").GetComponent<ShipController>().canPlayerMove = true;
    }
}
