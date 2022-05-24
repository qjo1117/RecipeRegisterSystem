using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecipeRegisterManager : MonoBehaviour
{
    static private RecipeRegisterManager _instance = null;
    static public RecipeRegisterManager Instance { get { if (_instance == null) { _instance = new RecipeRegisterManager(); } return _instance; } }

    public List<ItemInfo> _listInfos = new List<ItemInfo>();
    public List<RegisterSlot> _inventorys = new List<RegisterSlot>();
    public List<RegisterSlot> _recipes = new List<RegisterSlot>();
    public RegisterSlot _result = null;

    public RegisterSlot CurrentSlot = null;

    [SerializeField]
    public RecipeInfoJson _recipeInfos = new RecipeInfoJson();

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        Init();
    }

    private void Init()
	{
        LoadDirectoryData();
        InventoryCreate();
        RecipeCreate();

        InitInventory();

        _result = GameObject.Find("Result").gameObject.GetComponent<RegisterSlot>();
        _result._type = RegisterSlotType.Result;

        LoadFromJson();
    }

    public void ChangeSceneToRecipe()
	{
        SceneManager.LoadScene(2);
	}

    private void InitInventory()
	{
        for(int i = 0; i < _listInfos.Count; ++i) {
            _inventorys[i].Init(_listInfos[i]);
        }
        for (int i = _listInfos.Count; i < _inventorys.Count; ++i) {
            _inventorys[i].Init("null", 0);
        }
	}

    [ContextMenu("Files")]
    private void LoadDirectoryData()
	{
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "/Resources/Sprite");

        _listInfos.Clear();

        foreach (System.IO.FileInfo file in dir.GetFiles()) {
            string FileNameOnly = file.Name.Substring(0, file.Name.Length - 4);
            if (FileNameOnly.Split('.').Length == 1) {
                ItemInfo info = new ItemInfo();
                info.name = FileNameOnly;
                info.count = 99;
                _listInfos.Add(info);
            } 
        }
	}

    [ContextMenu("InventoryCreate")]
    public void InventoryCreate()
    {
        const int maxY = 3;
        const int maxX = 12;
        Vector2 basePos = new Vector2(-550, 100);

        Transform parent = GameObject.Find("InventoryPanel").transform;

        //Clear();
        for (int i = 0; i < maxY; ++i)
        {
            for (int j = 0; j < maxX; ++j)
            {
                GameObject slot = Instantiate(Resources.Load<GameObject>("RegisterSlot"), parent);
                int index = j + (int)(i * maxX);
                _inventorys.Add(slot.GetComponent<RegisterSlot>());
                slot.GetComponent<RegisterSlot>()._type = RegisterSlotType.Inventory;
                slot.gameObject.name = $"ItemSlot_{index}";
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + (j * 100.0f), basePos.y - (i * 100.0f));
            }
        }
    }

    [ContextMenu("RecipeCreate")]
    public void RecipeCreate()
    {
        Vector2 basePos = new Vector2(-200, 100);
        Transform parent = GameObject.Find("RecipePanel").transform;

        for (int i = 0; i < 9; ++i)
        {
            RegisterSlot slot = Instantiate(Resources.Load<GameObject>("RegisterSlot"), parent).GetComponent<RegisterSlot>();
            slot.gameObject.name = $"Combination_Slot_{i}";
            slot._type = RegisterSlotType.Combination;
            slot.GetComponent<RectTransform>().anchoredPosition = basePos + new Vector2((i % 3) * 100, (i / 3) * -100);
            slot.Init("null", 0);
            _recipes.Add(slot);
        }
    }

    [ContextMenu("Load")]
    public void LoadFromJson()
    {
        string jsonFile = File.ReadAllText(Application.dataPath + "/Resources/Data/SaveData.json");
        RecipeInfoJson infos = JsonUtility.FromJson<RecipeInfoJson>(jsonFile);
        _recipeInfos = infos;
    }

    [ContextMenu("Save")]
    public void SaveFromJson()
    {
        string json = JsonUtility.ToJson(_recipeInfos);
        CreateJsonFile(Application.dataPath + "/Resources/Data", "SaveData", json);
    }
    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }



    private string[] CombinationToKey()
    {
        string[] ret = new string[3];
        ret[0] = "";
        ret[1] = "";
        ret[2] = "";

        int baseX = 2, baseY = 2;
        int width = 3, height = 3;

        for (int x = 2; x >= 0; --x) {
            bool check = false;
            for (int y = 0; y < 3; ++y) {
                if (_recipes[x + (y * 3)]._info.name.Contains("null") == false) {
                    check = true;
                }
            }
            if (check && baseX > x) {     // Y의 기준점을 바꾼다.
                baseX = x;
            }
            else if (check == false) {
                width -= 1;
            }
        }

        for (int y = 2; y >= 0; --y) {
            bool check = false;
            for (int x = 0; x < 3; ++x) {
                if (_recipes[x + (y * 3)]._info.name.Contains("null") == false) {
                    check = true;
                }
            }
            if (check && baseY > y) {     // Y의 기준점을 바꾼다.
                baseY = y;
            }
            else if (check == false) {
                height -= 1;
            }
        }


        for (int i = 0; i < height; ++i) {
            for (int j = 0; j < width; ++j) {
                int x = baseX + j;
                int y = baseY + i;

                string str = _recipes[x + (y * 3)]._info.name;
                if (str != "null") {
                    ret[y] += str + "|";
                }
                else {
                    ret[y] += "|";
                }
            }
        }

        return ret;
    }

    public void RegisterRecipe()
	{
        RecipeInfo info = new RecipeInfo();
        info.retItem = _result._info;
        info.key = CombinationToKey();
        // 저거 Combination 문자열 가져와야함.
        _recipeInfos.listInfos.Add(info);
        SaveFromJson();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

