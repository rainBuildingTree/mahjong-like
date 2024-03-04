using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusCard : MonoBehaviour
{
    private int _bonusCardcode;
    private PlayerDeckManager _playerDeckManager;
    private const int NumTotalCardKind = 34;
    [SerializeField] private Sprite[] _cardSprites;
    [SerializeField] private Image bonusCardImage;

    public void Init()
    {
        bonusCardImage.enabled = false;
        _playerDeckManager = FindObjectOfType<PlayerDeckManager>();
    }
    private void Awake()
    {
        Init();
    }

    public void DrawNewBonusCard()
    {
        _bonusCardcode = Random.Range(0, NumTotalCardKind);
        bonusCardImage.enabled = true;
        bonusCardImage.sprite = _cardSprites[_bonusCardcode];
        _playerDeckManager.HuroController.CheckHuroability(_bonusCardcode);
            
    }
}
