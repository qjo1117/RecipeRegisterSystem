using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    public Button _deleteButton = null;
    public Button _createButton = null;

    public void Start()
	{
        _deleteButton.onClick.AddListener(OnDeleteButton);
        _createButton.onClick.AddListener(OnCreateButton);
    }

    public void OnCreateButton()
	{
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
	}

    public void OnDeleteButton()
	{
        InventoryManager.beforeSlot.Init("null", 0);
        gameObject.SetActive(false);
    }


}
