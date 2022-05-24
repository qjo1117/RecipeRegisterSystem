using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecipeInventory : MonoBehaviour
{
    static private RecipeInventory _instance = null;
    static public RecipeInventory Instance { get { if (_instance == null) { _instance = new RecipeInventory(); } return _instance; } }


    public List<RecipeSlot> _inventorys = new List<RecipeSlot>();
    public List<RecipeSlot> _recipes = new List<RecipeSlot>();
    public RecipeSlot _result = null;

    [SerializeField]
    public RecipeInfo _info;
    public RecipeInfoJson _recipeInfos = new RecipeInfoJson();

    public void PresentRecipe()
	{
        // 레시피 있는지 체크
        bool check = false;
        RecipeInfo current = null;
        foreach (RecipeInfo info in _recipeInfos.listInfos) {
            if(info.retItem.name == _result._info.name) {
                check = true;
                current = info;
                break;
            }
		}
        if(check == false) {
            for (int i = 0; i < 9; ++i) {
                _recipes[i].Init("null", 0);
            }
            return;
		}

        for (int i = 0; i < 9; ++i) {
            _recipes[i].Init("null", 0);
        }

        int index = 0;
        // 레시피를 보여주자
        for (int i = 0; i < 3; ++i) {
            string strcat = "";
            for (int j = 0; j < current.key[i].Length; ++j) {

                if(current.key[i][j] == '|') {
                    // 셋팅
                    ItemInfo info = new ItemInfo();
                    if(strcat == "") {
                        info.name = "null";
                        info.count = 0;
                    }
                    else {
                        info.name = strcat;
                        info.count = 1;
                    }


                    _recipes[index].Init(info);

                    // 초기화
                    strcat = "";
                    index += 1;
                }
                else {
                    strcat += current.key[i][j];
                }

            }
            index = (i + 1) * 3;
        }
	}

    public void ChangeSceneToInventory()
	{
        SceneManager.LoadScene(0);
	}

    public void ChangeSceneToRecipeRegister()
    {
        SceneManager.LoadScene(3);
    }

    public void SelectSlot(RecipeSlot target)
	{
        _result.Init(target._info.name, target._info.count);
        PresentRecipe();

    }

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        _result = GameObject.Find("Result").GetComponent<RecipeSlot>();

        // Make
        InventoryCreate();
        RecipeCreate();

        // Json
        LoadFromJson();

        // Data Insert
        for (int i = 0; i < _recipeInfos.listInfos.Count; ++i) {
            _inventorys[i].Make();
            _inventorys[i].Init(_recipeInfos.listInfos[i].retItem);
        }
        
    }

    [ContextMenu("RecipeCreate")]
    public void RecipeCreate()
    {
        Vector2 basePos = new Vector2(-200, 300);
        Transform parent = GameObject.Find("RecipePanel").transform;

        for (int i = 0; i < 9; ++i) {
            RecipeSlot slot = Instantiate(Resources.Load<GameObject>("RecipeSlot"), parent).GetComponent<RecipeSlot>();
            slot.gameObject.name = $"Combination_Slot_{i}";
            slot.GetComponent<RectTransform>().anchoredPosition = basePos + new Vector2((i % 3) * 100, (i / 3) * -100);
            slot.Init("null", 0);
            _recipes.Add(slot);
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
        for (int i = 0; i < maxY; ++i) {
            for (int j = 0; j < maxX; ++j) {
                GameObject slot = Instantiate(Resources.Load<GameObject>("RecipeSlot"), parent);
                int index = j + (int)(i * maxX);
                _inventorys.Add(slot.GetComponent<RecipeSlot>());
                slot.gameObject.name = $"ItemSlot_{index}";
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(basePos.x + (j * 100.0f), basePos.y - (i * 100.0f));
            }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
