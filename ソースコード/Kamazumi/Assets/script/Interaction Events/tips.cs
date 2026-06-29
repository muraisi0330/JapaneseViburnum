using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tips : InteractionEvents
{
   // public Text tipsText;
    public GameObject tipsToShow;
    public GameObject cube;
    bool isOnOff = false;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      
    }
   
    public override void interactionEvents()
    {
        if (cube.GetComponent<Player>().canControl == isOnOff) return;
        if (isOnOff == false)
        {
            cube.GetComponent<Player>().canControl = false;
            tipsToShow.SetActive(true);
            isOnOff = true;
        }
        else
        {
            cube.GetComponent<Player>().canControl = true;
            tipsToShow.SetActive(false);
            isOnOff = false;
        }
    }

    }
