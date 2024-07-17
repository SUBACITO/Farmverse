using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SearchController : MonoBehaviour
{
    public InputField searchInputField;  // Reference to the search input field
    public Button searchButton;          // Reference to the search button
    public GameObject itemsPanel;        // Reference to the panel containing the items
    public List<GameObject> items;       // List of all item GameObjects

    void Start()
    {
        // Populate the items list with child GameObjects of the itemsPanel
        foreach (Transform child in itemsPanel.transform)
        {
            items.Add(child.gameObject);
        }

        // Add a listener to the search button
        searchButton.onClick.AddListener(OnSearch);

        // Optionally, you can add a listener to trigger search when pressing enter
        searchInputField.onEndEdit.AddListener(OnSearchInputEndEdit);
    }

    void OnSearchInputEndEdit(string query)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSearch();
        }
    }

    void OnSearch()
    {
        string query = searchInputField.text;
        if (!string.IsNullOrEmpty(query))
        {
            FilterItems(query);
        }
        else
        {
            foreach (var item in items)
            {
                item.SetActive(true);
            }
        }
    }

    void FilterItems(string query)
    {
        foreach (var item in items)
        {
            if (item.name.ToLower().Contains(query.ToLower()))
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
}


