using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    private int _bonusCardcodes;
    private const int NumTotalCardKind = 34;
    [SerializeField] private Sprite[] _cardSprites;
    [SerializeField] private Image bonusCardImage;

    public void Init()
    {
        bonusCardImage.enabled = false;
    }
    private void Awake()
    {
        Init();
    }

    public void DrawNewBonusCard()
    {
            _bonusCardcodes = Random.Range(0, NumTotalCardKind);
            bonusCardImage.enabled = true;
            bonusCardImage.sprite = _cardSprites[_bonusCardcodes];
    }
}
