using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuroController : MonoBehaviour
{
    private PlayerDeckManager _manager;
    private PonButton _ponButton;
    private ChiButton _chiButton;
    private bool _isStraightable = true;
    private bool _isTripletable = false;
    private bool _isHuroPrepared = false;
    private List<(int, int)> _chiableCodesList = new List<(int, int)>();
    private (int, int) _hurosHandIndice;
    private int _bonusCardCode;
    private HuroSelector _selector;

    public bool IsHuroPrepared { get { return _isHuroPrepared; } set { _isHuroPrepared = value; } }
    public int BonusCardCode { get { return _bonusCardCode; } }
    public (int, int) HurosHandIndice { get { return _hurosHandIndice;} }   

    public void Init()
    {
        _manager = GetComponentInParent<PlayerDeckManager>();
        _ponButton = FindObjectOfType<PonButton>();
        _chiButton = FindObjectOfType<ChiButton>();
        _selector = FindObjectOfType<HuroSelector>();
        _chiButton.onClick.AddListener(() => PrepareChi());
        _ponButton.onClick.AddListener(() => PreparePon());
        _ponButton.gameObject.SetActive(false);
        _chiButton.gameObject.SetActive(false);
        _selector.Init();
    }
    public void CheckHuroability(int bonusCardCode)
    {
        _isStraightable = false;
        _isTripletable = false;
        _bonusCardCode = bonusCardCode;

        _isStraightable = IsStraightExist(bonusCardCode);
        _isTripletable = IsTripletExist(bonusCardCode);

        _chiButton.gameObject.SetActive(_isStraightable);
        _ponButton.gameObject.SetActive(_isTripletable);
    }
    public void PrepareChi()
    {
        if (!_isStraightable) return;
        if (_chiableCodesList.Count == 1)
        {
            ProcessChi();
            return;
        }
        for (int i = 0; i < _manager.Hand.Size; ++i)
            _manager.Hand.Cards[i].SetScreenerActive(true);
        _selector.gameObject.SetActive(true);
        _selector.Show(_chiableCodesList, _bonusCardCode);
        for (int i = 0; i < _selector.Buttons.Length; ++i)
        {
            int index = i;
            _selector.Buttons[index].onClick.RemoveAllListeners();
            if (i < _chiableCodesList.Count)
            {
                _selector.Buttons[index].onClick.AddListener(() => ThrowCardAway(_chiableCodesList[index].Item1, _chiableCodesList[index].Item2));
            }
        }

    }
    public void PreparePon()
    {
        if (!_isTripletable) return;
        ProcessPon();

    }
    private bool IsStraightExist(int bonusCardCode)
    {
        bool isStraightExist = false;
        _chiableCodesList.Clear();

        int targetAttribute = bonusCardCode / 9;
        int[] handCardsWithTargetAttribute = new int[9];
        for (int i = 0; i < handCardsWithTargetAttribute.Length; ++i)
            handCardsWithTargetAttribute[i] = 0;

        Card[] handCards = _manager.Hand.Cards;
        for (int i = 0; i < handCards.Length; ++i)
        {
            if (handCards[i].Code / 9 != targetAttribute) continue;
            handCardsWithTargetAttribute[handCards[i].Number - 1]++;
        }

        int[] cachedCandidates = new int[3];
        for (int i = 0; i < handCardsWithTargetAttribute.Length; ++i)
        {
            if (handCardsWithTargetAttribute[i] < 1) continue;
            for (int j = i+1; j < handCardsWithTargetAttribute.Length; ++j)
            {
                if (handCardsWithTargetAttribute[j] < 1) continue;
                cachedCandidates[0] = i;
                cachedCandidates[1] = j;
                cachedCandidates[2] = bonusCardCode % 9;
                Array.Sort(cachedCandidates);
                if (cachedCandidates[2] - cachedCandidates[1] == 1 && cachedCandidates[1] - cachedCandidates[0] == 1)
                {
                    isStraightExist = true;

                    _chiableCodesList.Add((9 * targetAttribute + i, 9 * targetAttribute + j));
                }
            }
        }
        return isStraightExist;
    }
    private bool IsTripletExist(int bonusCardCode)
    {
        Card[] handCards = _manager.Hand.Cards;
        for (int i = 0; i < handCards.Length - 1; ++i)
            if (handCards[i].Code == bonusCardCode && handCards[i+1].Code == bonusCardCode)
                return true;
        return false;
    }
    public void ThrowCardAway(int code0, int code1)
    {
        int i = 0;
        for (; i < _manager.Hand.Size; ++i)
        {
            _manager.Hand.Cards[i].SetScreenerActive(false);
            if (_manager.Hand.Cards[i].Code == code0)
            {
                _manager.Hand.Cards[i].SetScreenerActive(true);
                _hurosHandIndice.Item1 = i;
                ++i;
                break;
            }    
        }
        for (; i < _manager.Hand.Size; ++i)
        {
            if (_manager.Hand.Cards[i].Code == code1)
            {
                _manager.Hand.Cards[i].SetScreenerActive(true);
                _hurosHandIndice.Item2 = i;
                ++i;
                break;
            }
        }
        for (; i < _manager.Hand.Size; ++i)
        {
            _manager.Hand.Cards[i].SetScreenerActive(false);
        }
        _selector.Text.text = "Please Use One Card";
        _selector.Backgrounds[0].gameObject.SetActive(false);
        for (int j = 0; j < _selector.Buttons.Length; ++j)
        {
            _selector.Buttons[j].gameObject.SetActive(false);
        }
        _isHuroPrepared = true;
    }
    private void ProcessChi()
    {
        
    }
    private void ProcessPon()
    {

    }





}
