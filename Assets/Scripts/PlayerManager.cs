using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ItemHandler 
{
    public int index = 0;
    public ItemInfo info;
}

public class PlayerManager : MonoBehaviour
{
    public InventoryManager _inventory;
    public CombinationManager _combination;

    [SerializeField]
    public ItemJson _info;

    [System.Serializable]
    public class ItemJson {
        public List<ItemHandler> listInfos = new List<ItemHandler>();
	}

    [ContextMenu("Inventory Make")]
    public void InventorySeting()
	{
        _inventory.ClearSlot();
        for (int i = 0; i < _info.listInfos.Count; ++i) {
            _inventory.SetSlot(
                _info.listInfos[i].index % InventoryManager._maxX,
                _info.listInfos[i].index / InventoryManager._maxX,
                _info.listInfos[i].info);
        }
	}

    [ContextMenu("Sort")]
    private void ListSort()
	{
        _info.listInfos.Sort((target, other) => target.index.CompareTo(other.index));
	}

    [ContextMenu("Save")]
    public void SaveFromJson()
    {
        string json = JsonUtility.ToJson(_info);
        CreateJsonFile("Assets/Resources/Data", "PlayerSave", json);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    [ContextMenu("CreateManager")]
    void CreateManager()
	{
        _inventory.Create();
        _combination.Create();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
