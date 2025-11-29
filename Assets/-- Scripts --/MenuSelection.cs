using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds_5 = new WaitForSeconds(.5f);

    [Header("-- UI --")]
    public Button startBtn;
    public Animation menuCloseAnim;
    public GridLayoutGroup gridLayoutGroup;
    public Image[] allAvailable_IMAGES;
    public int[] cardsRatio_OPT;

    private int selectedCARDIs;

    [Header("-- Audio Setup --")]
    public AudioSource audiSRC;
    public AudioClip tapSound_CLIP;

    void OnEnable()
    {
        for (int i = 0; i < allAvailable_IMAGES.Length; i++)
        {
            allAvailable_IMAGES[i].gameObject.SetActive(false);
        }

        startBtn.interactable = false;
        gridLayoutGroup.enabled = true;
        menuCloseAnim.playAutomatically = true;
    }

    public void TapOnMenu_CARDS(int index) // Calling From 3 Cards MENU
    {
        selectedCARDIs = index;
        audiSRC.PlayOneShot(tapSound_CLIP);
        startBtn.interactable = true;
    }

    public void TapOn_StartBtn() // Calling From Start Btn
    {
        StartCoroutine(StartGame_Now());
    }

    private IEnumerator StartGame_Now()
    {
        GameManager.insta.allImages.Clear();
        for (int i = 0; i < cardsRatio_OPT[selectedCARDIs]; i++)
        {
            allAvailable_IMAGES[i].gameObject.SetActive(true);
            GameManager.insta.allImages.Add(allAvailable_IMAGES[i]);
        }

        gridLayoutGroup.constraintCount = cardsRatio_OPT[selectedCARDIs] / 2;

        menuCloseAnim.Play("MenuClose");

        GameManager.insta.PlayGame();
        yield return _waitForSeconds_5;
        gridLayoutGroup.enabled = false;
        menuCloseAnim.gameObject.SetActive(false);
    }
}