using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum RegisterSlotType 
{
    Inventory,
    Combination,
    Result,
}


public class RegisterSlot : MonoBehaviour, IPointerDownHandler 
{
    public ItemInfo _info;

    public Image _image = null;
    public Text _text = null;

    public RegisterSlotType _type = RegisterSlotType.Inventory;

    // Start is called before the first frame update
    void Start()
    {
        if (_image == null) {
            Make();
        }
    }

    public void Make()
    {
        _info = new ItemInfo();
        _info.name = "null";
        _info.count = 0;

        _image = transform.GetChild(1).GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
    }

    public void Init(string name, int count)
    {
        Make();

        if (name.Contains("null") == false)
        {
            _image.sprite = Resources.Load<Sprite>("Sprite/" + name);
        }
        else
        {
            _image.sprite = Resources.Load<Sprite>("Sprite/null");
        }
        _text.text = count.ToString();

        _info.name = name;
        _info.count = count;
    }

    public void Init(ItemInfo info)
    {
        Init(info.name, info.count);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnPointerDown(PointerEventData eventData)
	{


        if (_type == RegisterSlotType.Inventory) {
            RecipeRegisterManager.Instance.CurrentSlot = this;
        }
        else if (_type == RegisterSlotType.Combination){
            if(RecipeRegisterManager.Instance.CurrentSlot != null) {
                if (Input.GetMouseButtonDown(1)) {
                    if ((_info.name.Contains("null") || _info.name == "") == false) {
                        Init("null", 0);
                    }
                }
                else {
                    ItemInfo info = RecipeRegisterManager.Instance.CurrentSlot._info;
                    info.count = 1;
                    Init(info);
                }

			}
		}
        else if (_type == RegisterSlotType.Result) {
            if (_info.name.Contains(RecipeRegisterManager.Instance.CurrentSlot._info.name)) {
                ItemInfo info = _info;
                info.count += 1;
                Init(info);
            }
            else {
                ItemInfo info = RecipeRegisterManager.Instance.CurrentSlot._info;
                info.count = 1;
                Init(info);
            }
        }
    }
}
