using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	[SerializeField]
    public Dictionary<string, ItemInfo> _listItemInfo = new Dictionary<string, ItemInfo>();

	public InventoryManager _inventory;

	public RecipeManager _recipeManager = null;


	private void Start()
	{
		_recipeManager = FindObjectOfType<RecipeManager>();
		_inventory = FindObjectOfType<InventoryManager>();

	}

	private void Update()
	{
	}

	public void LoadFromData(string path)
	{

	}

	public void OnLoad()
	{
		
	}

	[ContextMenu("Test")]
	public void TestInit()
	{
		ItemInfo info;

		{
			info = new ItemInfo();
			info.name = "0";
			info.count = 10;
			_listItemInfo.Add("0", info);
		}

		{
			info = new ItemInfo();
			info.name = "1";
			info.count = 10;
			_listItemInfo.Add(info.name, info);
		}


		{
			info = new ItemInfo();
			info.name = "2";
			info.count = 10;
			_listItemInfo.Add(info.name, info);
		}

		{
			info = new ItemInfo();
			info.name = "3";
			info.count = 4;
			_listItemInfo.Add(info.name, info);
		}
	}

	[ContextMenu("Test1")]
	public void TestInit2()
	{
		ItemInfo info;

		{
			info = new ItemInfo();
			info.name = "Book";
			info.count = 2;
			_listItemInfo.Add("0", info);
		}
	}

}
