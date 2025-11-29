using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static readonly WaitForSeconds Wait2s = new WaitForSeconds(2f);
    private static readonly WaitForSeconds Wait1s = new WaitForSeconds(1f);

    public static GameManager insta;

    private void Awake()
    {
        if (insta == null)
        {
            insta = this;
        }
        else if (insta != this)
        {
            Destroy(gameObject);
        }
    }

    [Header("-- Card Setup --")]
    public int[] randomArrayIndex;
    public Sprite[] allAnimals;
    public Sprite backSideCARD;
    public List<Image> allImages;
    public Button[] allCardBtn;

    [Header("-- UI --")]
    public TextMeshProUGUI matchesTxt;
    public TextMeshProUGUI turnsTxt;

    [Header("-- Game State --")]
    public int matchsCNT;
    public int turnsCNT;

    [Header("-- Audio Setup --")]
    public AudioSource audiSRC;
    public AudioClip tapOnCard_CLIP, wrongTap_CLIP,correctTap_CLIP;

    // All Prvt Flds...
    private List<Sprite> cardSprites = new List<Sprite>();
    private System.Random _random = new System.Random();

    private int firstIndex = -1;
    private int secondIndex = -1;
    private bool inputLocked = false;


    private void Start()
    {
        matchsCNT = SaveManager.TotalMatches;
        turnsCNT = SaveManager.TotalTurns;

        UpdateCounterText(matchesTxt, matchsCNT);
        UpdateCounterText(turnsTxt, turnsCNT);
    }


    public void PlayGame()
    {
        StartCoroutine(FirstShow());
    }


    private IEnumerator FirstShow()
    {
        cardSprites.Clear();
        firstIndex = -1;
        secondIndex = -1;
        inputLocked = true;

        UpdateCounterText(turnsTxt, turnsCNT);
        UpdateCounterText(matchesTxt, matchsCNT);

        for (int i = 0; i < allImages.Count; i++)
        {
            allImages[i].sprite = backSideCARD;
            allImages[i].gameObject.SetActive(true);
            allCardBtn[i].interactable = false;
        }

        yield return Wait1s;

        ShuffleArray(randomArrayIndex);

        int pairCount = allImages.Count / 2;

        for (int i = 0; i < pairCount; i++)
        {
            Sprite s = allAnimals[randomArrayIndex[i]];
            cardSprites.Add(s);
            cardSprites.Add(s);
        }

        for (int i = 0; i < cardSprites.Count; i++)
        {
            int r = _random.Next(i, cardSprites.Count);
            (cardSprites[i], cardSprites[r]) = (cardSprites[r], cardSprites[i]);
        }

        for (int i = 0; i < allImages.Count; i++)
        {
            allImages[i].sprite = cardSprites[i];
        }

        yield return Wait2s;

        for (int i = 0; i < allImages.Count; i++)
        {
            allImages[i].sprite = backSideCARD;
            allCardBtn[i].interactable = true;
        }

        inputLocked = false;
    }


    private void ShuffleArray(int[] tempStore)
    {
        int p = tempStore.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = _random.Next(0, n + 1);
            int t = tempStore[r];
            tempStore[r] = tempStore[n];
            tempStore[n] = t;
        }
    }


    public void TapOnCards_Index(int index)
    {
        // All Validation Before Tap On CARDS
        if (inputLocked)
            return;

        if (!allCardBtn[index].interactable)
            return;

        if (index == firstIndex)
            return;

        StartCoroutine(CheckSet_Card(index));
    }



    private IEnumerator CheckSet_Card(int index)
    {
        audiSRC.PlayOneShot(tapOnCard_CLIP);
        allImages[index].sprite = cardSprites[index];

        if (firstIndex == -1)
        {
            firstIndex = index;
            yield break;
        }

        secondIndex = index;
        inputLocked = true;

        turnsCNT++;
        SaveManager.TotalTurns = turnsCNT; // Saving Turns
        SaveManager.SaveNow();

        UpdateCounterText(turnsTxt, turnsCNT);

        yield return Wait1s;

        if (cardSprites[firstIndex] == cardSprites[secondIndex])
        {
            // Correct MATCHED
            audiSRC.PlayOneShot(correctTap_CLIP);

            matchsCNT++;
            SaveManager.TotalMatches = matchsCNT; // Saving Matches
            SaveManager.SaveNow();

            UpdateCounterText(matchesTxt, matchsCNT);

            allImages[firstIndex].gameObject.SetActive(false);
            allImages[secondIndex].gameObject.SetActive(false);
            allCardBtn[firstIndex].interactable = false;
            allCardBtn[secondIndex].interactable = false;
        }
        else
        {
            // Wrong Matched
            audiSRC.PlayOneShot(wrongTap_CLIP);

            allImages[firstIndex].sprite = backSideCARD;
            allImages[secondIndex].sprite = backSideCARD;
        }

        firstIndex = -1;
        secondIndex = -1;
        inputLocked = false;
    }

    public void ResetAll_Prefs() // Called From RESET Button
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void UpdateCounterText(TextMeshProUGUI txtProDisp, int value)
    {
        txtProDisp.text = value.ToString();
    }
}
