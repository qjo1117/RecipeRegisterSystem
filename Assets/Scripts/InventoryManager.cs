using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public ItemSlot[] _slots = new ItemSlot[_maxX * _maxY];

    public Vector3 _basePos;
    public float _height = 0.0f;
    public float _width = 0.0f;

    static public ItemInfo currentHandler;
    static public ItemSlot beforeSlot;
    static public ItemSlot currentSlot;
    static public Transform InventoryTransform;
    static public bool isFocus = false;
    static public SlotButton SlotButton;

    private Canvas _canvas;

    [SerializeField]
    public const int _maxX = 12;
    [SerializeField]
    public const int _maxY = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentSlot = GameObject.Instantiate(Resources.Load<GameObject>("ItemSlot"), transform).GetComponent<ItemSlot>();

        InventoryTransform = transform;
        _canvas= GameObject.FindObjectOfType<Canvas>();

        SlotButton = GameObject.Find("SlotButton").GetComponent<SlotButton>();
    }

    void Update()
    {
    }

    public void ClearSlot()
	{
        for (int i = 0; i < _maxX * _maxY; ++i) {
            _slots[i].Init("null", 0);
        }
	}
    public void SetSlot(int x, int y, ItemInfo info)
    {
        _slots[x + (y * _maxX)].Init(info);
    }

    [ContextMenu("Init_Set")]
    public void SetingSlots()
    {
        for (int i = 0; i < _maxY; ++i)
        {
            for (int j = 0; j < _maxX; ++j)
            {
                _slots[j + (i * _maxX)].Init("null", 0);
            }
        }
    }

    [ContextMenu("Create")]
	public void Create()
	{
        //Clear();
        for (int i = 0; i < _maxY; ++i) {
            for (int j = 0; j < _maxX; ++j) {
                GameObject slot = Instantiate(Resources.Load<GameObject>("ItemSlot"), transform);
                int index = j + (int)(i * _maxX);
                _slots[index] = slot.GetComponent<ItemSlot>();
                slot.gameObject.name = $"ItemSlot_{index}";
                slot.GetComponent<ItemSlot>()._index = index;
                slot.GetComponent<ItemSlot>().Init("null", 0);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(_basePos.x + (j * _width), _basePos.y - (i * _height));
            }
        }
    }

 //   [ContextMenu("Clear")]
 //   public void Clear()
	//{
 //       _slots.Clear();
 //       for (int i = 0;i<_slots.Count;++i) {
 //           GameObject.Destroy(_slots[i].gameObject);
 //       }
 //   }

	// Update is called once per frame

}
