using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds_5 = new WaitForSeconds(.5f);
    public Button startBtn;
    public int selectedCARDIs;
    public Animation menuCloseAnim;

    public Image[] allAvailable_IMAGES;
    public int[] cardsRatio_OPT;

    public GridLayoutGroup gridLayoutGroup;

    void OnEnable()
    {
        for (int i = 0; i < allAvailable_IMAGES.Length; i++)
        {
            allAvailable_IMAGES[i].gameObject.SetActive(false);
        }

        startBtn.interactable = true;
        gridLayoutGroup.enabled = true;
        menuCloseAnim.playAutomatically = true;
    }

    public void TapOnMenu_CARDS(int index)
    {
        selectedCARDIs = index;
    }

    public void TapOn_StartBtn()
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