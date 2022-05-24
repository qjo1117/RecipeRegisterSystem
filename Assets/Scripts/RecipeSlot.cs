using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerDownHandler
{
    public ItemInfo _info;

    public Image _image = null;
    public Text _text = null;

    // Start is called before the first frame update
    void Start()
    {
        if(_image == null)
		{
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

        if (name.Contains("null") == false) {
            _image.sprite = Resources.Load<Sprite>("Sprite/" + name);
        }
        else {
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
        RecipeInventory.Instance.SelectSlot(this);
    }
}
