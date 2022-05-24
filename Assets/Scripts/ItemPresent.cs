using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemPresent : MonoBehaviour
{
    public ItemJson _itemInfos = new ItemJson();

    public Image _itemImage = null;
    public Text _itemText = null;
    public Text _itemCount = null;
    public Text _itemIndex = null;
    public Button _exacuteButton = null;

    public ItemRegister[] _itemRegister = new ItemRegister[3];

	private void Start()
	{
        LoadFromJson();
        _exacuteButton.onClick.AddListener(Present);
    }

    private void Update()
	{
        for (int i = 0; i < _itemRegister.Length; i++) {
            _itemRegister[i].Present();
        }

        _itemImage.sprite = Resources.Load<Sprite>("Sprite/" + _itemRegister[0].StringRegister);
        _itemText.text = $"Name : {_itemRegister[0].StringRegister}";
        _itemCount.text = $"Count : {_itemRegister[1].StringRegister}";

        int index = 0;
        if (int.TryParse(_itemRegister[2].StringRegister, out index))
        {

            _itemIndex.text = $"Index : {index}";
        }
    }

    private bool CheckInventory()
	{
        const int size = 35;
        bool[] checkIndex = new bool[size];
        for(int i = 0; i < size; i++) {
            checkIndex[i] = false;
		}

        for (int i = 0; i < _itemInfos.listInfos.Count; ++i) {
            if (checkIndex[_itemInfos.listInfos[i].index] == false) {
                checkIndex[_itemInfos.listInfos[i].index] = true;
            }
            else {
                return false;
			}
        }
        return true;
    }

	public void Present()
	{
        for(int i = 0; i < _itemRegister.Length; i++) {
            _itemRegister[i].Present();
        }

        if(CheckInventory() == false) {      // 가능여부
            return;
		}

        _itemImage.sprite = Resources.Load<Sprite>("Sprite/" + _itemRegister[0].StringRegister);
        _itemText.text = $"Name : {_itemRegister[0].StringRegister}";
        _itemCount.text = $"Count : {_itemRegister[1].StringRegister}";

        int index = 0;
        if(int.TryParse(_itemRegister[2].StringRegister, out index))  {
            
            _itemIndex.text = $"Index : {index}";
        }

        ItemHandler itemInfo = new ItemHandler();

        itemInfo.info.name = _itemRegister[0].StringRegister;
        int.TryParse(_itemRegister[1].StringRegister, out itemInfo.info.count);
        int.TryParse(_itemRegister[2].StringRegister, out itemInfo.index);
        _itemInfos.listInfos.Add(itemInfo);

        SaveFromJson();

        SceneManager.LoadScene(0);
    }

    public void SaveFromJson()
    {
        string json = JsonUtility.ToJson(_itemInfos);
        CreateJsonFile(Application.dataPath + "/Resources/Data", "PlayerSave", json);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void LoadFromJson()
    {
        string jsonFile = File.ReadAllText(Application.dataPath + "/Resources/Data/PlayerSave.json");
        ItemJson infos = JsonUtility.FromJson<ItemJson>(jsonFile);
        _itemInfos = infos;

    }
}
