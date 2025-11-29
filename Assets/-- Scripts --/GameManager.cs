using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager insta;
    void Awake()
    {
        if(insta == null)
        {
            insta = this;
        }
    }

    public Sprite [] allAnimals;
    public Image [] allImages;

    public void TapOnCards_Index(int index)
    {
        
    }
}