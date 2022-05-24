using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRegister : MonoBehaviour
{
    public string _strRegister = "";
    public string StringRegister { get { return _strRegister; } }

    public InputField _inputField = null;

    void Start()
    {
        _inputField = GetComponentInChildren<InputField>();
    }

    public void Present()
	{
        _strRegister = _inputField.text;
	}
    
}
