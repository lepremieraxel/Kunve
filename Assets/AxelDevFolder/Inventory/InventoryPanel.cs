using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private List<GameObject> caseList;
    [SerializeField] private List<GameObject> usedCase;
    [SerializeField] private List<ItemData> displayedItem;

    private void Update()
    {
        DisplayInventory();
    }

    void DisplayInventory()
    {
        displayedItem.Clear();
        caseList.Clear();
        usedCase.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            caseList.Add(transform.GetChild(i).gameObject);
        }

        foreach(GameObject inventoryCase in caseList)
        {
            inventoryCase.transform.GetChild(0).GetComponent<Image>().sprite = null;
            inventoryCase.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
            inventoryCase.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        }

        if (inventory.content != null)
        {
            foreach (ItemData item in inventory.content)
            {
                if (!displayedItem.Contains(item))
                {
                    displayedItem.Add(item);
                    DisplayInventoryCase(item);
                }
                else
                {
                    UpdateInventoryCase(displayedItem.IndexOf(item));
                }
            }
        }
    }

    void DisplayInventoryCase(ItemData item)
    {
        usedCase.Add(caseList[usedCase.Count]);
        GameObject currentCase = usedCase.Last();
        Image currentCaseImage = currentCase.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI currentCaseText = currentCase.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        currentCaseImage.sprite = item.visual;
        currentCaseImage.color = Color.white;
        currentCaseText.text = "1";
    }

    void UpdateInventoryCase(int index)
    {
        GameObject currentCase = usedCase[index];
        TextMeshProUGUI currentCaseText = currentCase.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        int currentCount = int.Parse(currentCaseText.text) + 1;

        currentCaseText.text = currentCount.ToString();
    }
}
