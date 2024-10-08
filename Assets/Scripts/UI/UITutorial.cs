using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITutorial : MonoBehaviour
{

    // Currently hardcoded only for Fried Egg !!

    public string[] instructions;
    public GameObject[] thingsToUnhide;

    public TextMeshProUGUI instructionTextbox;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in thingsToUnhide)
        {
            obj.SetActive(false);
        }

        if (FriedEggManager.main == null)
            Disable();

        if (OrderManager.Instance.orderCounter > 0)
            Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (OrderManager.Instance.orderCounter > 0)
            Disable();

        instructionTextbox.text = instructions[FriedEggManager.main.currentStage];
    }

    void Disable()
    {
        foreach (GameObject obj in thingsToUnhide)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
