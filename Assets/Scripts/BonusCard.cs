using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    private int[] _bonusCardcodes;
    private const int NumMaxBonusCard = 4;
    private const int NumTotalCardKind = 34;
    [SerializeField] private Sprite[] _cardSprites;
    [SerializeField] private Image[] _images;

    public void Init()
    {
        _bonusCardcodes = new int[NumMaxBonusCard];
        for (int i = 0; i < _images.Length; ++i)
        {
            _images[i].enabled = false;
        }
    }
    private void Awake()
    {
        Init();
    }

    public void DrawNewBonusCards()
    {
        for (int i = 0; i < _bonusCardcodes.Length; ++i)
        {
            _bonusCardcodes[i] = Random.Range(0, NumTotalCardKind);
            _images[i].enabled = true;
            _images[i].sprite = _cardSprites[_bonusCardcodes[i]];
        }
    }
}
