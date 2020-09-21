using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _gemText;

    public void UpateGemDisplay(int gems)
    {
        _gemText.text = gems.ToString();
    }
}
