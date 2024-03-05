using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonDisplayBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private List<GameObject> buttonDisplayed = new List<GameObject>();

    private void Start()
    {
        DisplayButton("Inventory", "Inventaire");
    }
    private void Update()
    {
        foreach(GameObject button in buttonDisplayed)
        {
            button.transform.GetComponentInChildren<Image>().sprite = player.GetInputButtonImage(button.name);
        }
    }

    public void DisplayButton(string buttonName, string actionName)
    {
        GameObject button = Instantiate(buttonPrefab, transform);
        button.name = buttonName;
        button.transform.GetComponentInChildren<Image>().sprite = player.GetInputButtonImage(buttonName);
        button.transform.GetComponentInChildren<TextMeshProUGUI>().text = actionName;
        buttonDisplayed.Add(button);
    }
    public void UndisplayButton(string buttonName)
    {
        GameObject buttonToRemove = null;
        foreach (GameObject button in buttonDisplayed)
        {
            if(button.name == buttonName)
            {
                buttonToRemove = button;
            }
        }
        buttonDisplayed.Remove(buttonToRemove);
        Destroy(buttonToRemove);
    }
}
