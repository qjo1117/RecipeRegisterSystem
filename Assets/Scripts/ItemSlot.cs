using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct ItemInfo 
{
	public string name;
	public int count;
};

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField]
    public ItemInfo _itemInfo;
	public Image _image = null;
	public Text _text = null;
	public Sprite _sprite = null;
	public int _index = -1;
	public Canvas _canvas = null;

	public CanvasGroup _canvasGroup = null;
	public CombinationManager _combinationManager = null;

	ItemInfo ItemInfo { get { return _itemInfo; } set { _itemInfo = value; } }

	private void Start()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvas = GameObject.FindObjectOfType<Canvas>();
		_combinationManager = GameObject.FindObjectOfType<CombinationManager>();
	}

	public void Init()
	{
		string path = $"Sprite/{_itemInfo.name}";
		Sprite texture = Resources.Load<Sprite>(path);
		if(texture == null) {
			return;
		}

		_text.text = $"{_itemInfo.count}";
		_image.sprite = texture;
		_sprite = texture;
	}

	public void Init(string name, int count)
	{
		string path = $"Sprite/{name}";
		_sprite = Resources.Load<Sprite>(path);
		_itemInfo.count = count;
		_itemInfo.name = name;

		_image.sprite = _sprite;
		_text.text = $"{_itemInfo.count}";
	}

	public void Init(ItemInfo info)
	{
		string path = $"Sprite/{info.name}";
		_image.sprite = Resources.Load<Sprite>(path);
		_sprite = _image.sprite;
		_itemInfo.count = info.count;
		_itemInfo.name = info.name;
		_text.text = $"{_itemInfo.count}";
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (InventoryManager.isFocus == false) {
			if(_itemInfo.count == 0) {
				return;
			}

			InventoryManager.currentSlot.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
			InventoryManager.currentSlot.gameObject.SetActive(true);
			InventoryManager.currentSlot.GetComponent<CanvasGroup>().interactable = false;
			InventoryManager.currentSlot.GetComponent<CanvasGroup>().blocksRaycasts = false;
			InventoryManager.isFocus = true;

			InventoryManager.beforeSlot = this;

			if(Input.GetKey(KeyCode.LeftControl)) {
				if(_itemInfo.count != 0) {
					_itemInfo.count -= 1;
				}
				ItemInfo info = _itemInfo;
				info.count = 1;
				InventoryManager.currentSlot.Init(info);

				if(_itemInfo.count <= 0) {
					_itemInfo.name = "null";
					_itemInfo.count = 0;
				}
			}
			else
			{
				InventoryManager.currentSlot.Init(ItemInfo);
				_itemInfo.name = "null";
				_itemInfo.count = 0;
			}
			Init();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (InventoryManager.isFocus)
		{
			InventoryManager.currentSlot.GetComponent<RectTransform>().anchoredPosition += eventData.delta / _canvas.scaleFactor;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (InventoryManager.isFocus) {

			ItemSlot parent = eventData.pointerEnter.GetComponentInParent<ItemSlot>();
			if(parent == null || parent.gameObject.name.Contains("Result")) {
				InventoryManager.beforeSlot.ItemInfo = InventoryManager.currentSlot.ItemInfo;
				InventoryManager.beforeSlot.Init();
				InventoryManager.currentSlot.GetComponent<CanvasGroup>().interactable = true;
				InventoryManager.currentSlot.GetComponent<CanvasGroup>().blocksRaycasts = true;
				InventoryManager.currentSlot.gameObject.SetActive(false);
				InventoryManager.isFocus = false;
				return;
			}

			if (parent.ItemInfo.count != 0) {
				if (parent.ItemInfo.name.Contains(InventoryManager.currentSlot.ItemInfo.name)) {
					ItemInfo info = InventoryManager.currentSlot.ItemInfo;
					ItemInfo parentInfo = parent.ItemInfo;
					info.count += parent.ItemInfo.count;
					parent.Init(info);


				}
				else {
					ItemInfo info = InventoryManager.currentSlot.ItemInfo;
					ItemInfo parentInfo = parent.ItemInfo;
					parent.Init(info);
					InventoryManager.beforeSlot.Init(parentInfo);
				}

			}
			else {
				ItemInfo info = InventoryManager.currentSlot.ItemInfo;
				parent.Init(info);
			}
			if (InventoryManager.beforeSlot.gameObject.name.Contains("Result")) {
				_combinationManager.DisCountSlot();
			}

			InventoryManager.currentSlot.GetComponent<CanvasGroup>().interactable = true;
			InventoryManager.currentSlot.GetComponent<CanvasGroup>().blocksRaycasts = true;
			InventoryManager.currentSlot.gameObject.SetActive(false);
			InventoryManager.isFocus = false;
			CombinationManager.instance.Combination();
		}
	}
}
