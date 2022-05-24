using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RecipeInfo
{
    public ItemInfo retItem;
    public Dictionary<string, RecipeInfo> dict = new Dictionary<string, RecipeInfo>();
    public string[] key = new string[3];
}

[System.Serializable]
public class RecipeInfoJson {
    public List<RecipeInfo> listInfos = new List<RecipeInfo>();

}

public class RecipeManager : MonoBehaviour
{
    [SerializeField]
    public RecipeInfo _info;

    public RecipeInfoJson _listInfos = new RecipeInfoJson();


    public string[] _keys;

	private void Start()
	{
        LoadFromJson();

    }

	public RecipeInfo FindItemInfo(RecipeInfo iter, string[] keys, int index = 0)
	{
        if(index >= 3) {
            return iter;
		}

        if(keys[index] != "") {
            if (iter.dict.ContainsKey(keys[index]) == false) {
                return null;
            }
            else {
                return FindItemInfo(iter.dict[keys[index]], keys, index + 1);
            }
        } 
        else {
            return FindItemInfo(iter, keys, index + 1);
        }
	}

	[ContextMenu("Save")]
	public void SaveFromJson()
	{ 
		string json = JsonUtility.ToJson(_listInfos);
        CreateJsonFile(Application.dataPath + "/Resources/Data", "SaveData", json); 
    }

	void CreateJsonFile(string createPath, string fileName, string jsonData)
	{
		FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
		byte[] data = Encoding.UTF8.GetBytes(jsonData);
		fileStream.Write(data, 0, data.Length);
		fileStream.Close();
	}

    [ContextMenu("Load")]
	public void LoadFromJson()
	{
        string jsonFile = File.ReadAllText(Application.dataPath + "/Resources/Data/SaveData.json");
        RecipeInfoJson infos = JsonUtility.FromJson<RecipeInfoJson>(jsonFile);
        _listInfos = infos;

        for (int i = 0; i < _listInfos.listInfos.Count; i++) {
            LoadRegisterRecipe(_listInfos.listInfos[i].key, _listInfos.listInfos[i].retItem);
        }
    }

    [ContextMenu("CreateInfoDict1")]
    public void TestCreateInfo2()
	{
        {
            string[] key = new string[3];
            key[0] = "BookBook";
            key[1] = "Book1";
            key[2] = "";
            RegisterRecipe(key, new ItemInfo { count = 1, name = "Carrot" });
        }
    }

    [ContextMenu("TestFindItemInfo")]
    public void TestFindInfo()
	{
        RecipeInfo info = FindItemInfo(_info, _keys, 0);
        if (info == null)
        {
            Debug.Log("없음");
        }
        else
        {
            Debug.Log(info.retItem.name);
            Debug.Log(info.retItem.count);
        }

    }

    public void ChangeSceneToRecipe()
	{
        SceneManager.LoadScene(2);
	}

	void RegisterRecipe(string[] keys, ItemInfo info)
	{
        RecipeInfo iter = _info;

        for (int i = 0; i < 3; ++i)
        {
            if(keys[i] != "")
			{
                // 없을경우 생성
                if(iter.dict.ContainsKey(keys[i]) == false)
				{
                    iter.dict.Add(keys[i], new RecipeInfo());
                }
                iter = iter.dict[keys[i]];
                
            }
        }

        iter.key = keys;
        iter.retItem = info;
        _listInfos.listInfos.Add(iter);
    }

    void LoadRegisterRecipe(string[] keys, ItemInfo info)
	{
        RecipeInfo iter = _info;

        for (int i = 0; i < 3; ++i)
        {
            if (keys[i] != "")
            {
                // 없을경우 생성
                if (iter.dict.ContainsKey(keys[i]) == false)
                {
                    iter.dict.Add(keys[i], new RecipeInfo());
                }
                iter = iter.dict[keys[i]];

            }
        }

        if (iter.retItem.count == 0)
        {
            iter.key = keys;
            iter.retItem = info;
        }
    }
}
