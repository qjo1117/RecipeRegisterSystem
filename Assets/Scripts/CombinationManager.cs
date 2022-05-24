using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationManager : MonoBehaviour
{
    static public CombinationManager instance;

    public Vector3 _basePos = Vector3.zero;
    public float _width = 10.0f;
    public float _height = 10.0f;

    public ItemSlot[] _slots = new ItemSlot[9];

    public RecipeManager _recipeManager = null;

    public ItemSlot _retImage = null;
    public Image _arrowImage = null;

    void Start()
    {
        instance = this;
        _recipeManager = FindObjectOfType<RecipeManager>();
    }

    [ContextMenu("Combination")]
    public void Combination()
	{
        string[] findStr = CombinationToString();
        RecipeInfo info = _recipeManager.FindItemInfo(_recipeManager._info, findStr);
        if(info != null && info.retItem.count != 0) {
            _retImage._itemInfo = info.retItem;
            _retImage.Init();
        }
        else {
            _retImage.Init("null", 0);
		}
    }


    public void DisCountSlot()
    {
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (_slots[j + (i * 3)]._itemInfo.count != 0) {
                    _slots[j + (i * 3)]._itemInfo.count -= 1;
                    if(_slots[j + (i * 3)]._itemInfo.count == 0) {
                        _slots[j + (i * 3)].Init("null", 0);
                    }
                    else {
                        _slots[j + (i * 3)].Init();
                    }
                }
            }
        }
    }

    public string[] CombinationToString()
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
                if(_slots[x + (y * 3)]._itemInfo.name.Contains("null") == false) {
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
                if (_slots[x + (y * 3)]._itemInfo.name.Contains("null") == false) {
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

                string str = _slots[x + (y * 3)]._itemInfo.name;
                if (str != "null") {
                    ret[y] += str + "|";
                }
                else {
                    ret[y] += "|";
                }
            }
        }

        //Debug.Log($"X : {baseX}, Y : {baseY} / Width : {width}, Height : {height}");
        //Debug.Log($"Ret [{ret[0]}] [{ret[1]}] [{ret[2]}]");

        return ret;
	}

    [ContextMenu("Create")]
    public void Create()
	{
        for (int i = 0; i < 9; ++i) {
            _slots[i] = Instantiate(Resources.Load<GameObject>("ItemSlot"), transform).GetComponent<ItemSlot>();
            _slots[i].name = $"Combination_Slot_{i}";
            _slots[i].GetComponent<RectTransform>().anchoredPosition = _basePos + new Vector3((i % 3) * _width, (i / 3) * -_height, 0.0f);
            _slots[i].Init("null", 0);
        }
	}

	void Update()
    {
        
    }
}
