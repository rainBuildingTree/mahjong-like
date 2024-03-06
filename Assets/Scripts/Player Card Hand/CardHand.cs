using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardHand : MonoBehaviour {
    
    public const int NumTotalCardKind = 34;
    protected const int NumCardKindPerAttribute = 9;
    protected PlayerDeckManager _manager;
    protected int _size;
    protected Card[] _cards;
    protected List<int> _requiredCards = new List<int>();
    protected int _numCards;
    protected int _unluckCount = 0;

    public int Size { get { return _size; } }
    public Card[] Cards { get { return _cards; } }
    public int NumCards { get { return _numCards; } }

    protected void Awake() {
        _manager = GetComponent<PlayerDeckManager>();
    }
    public void Init(int size = 14) {
        _size = size;
        _numCards = 0;
        _cards = new Card[_size];
        CreateCards();
        Sort();
    }
    public void CreateCards() {
        for (int i = 0; i < _size; ++i) {
            _cards[i] = Instantiate(_manager.CardPrefab, transform).GetComponent<Card>();
            _cards[i].Initialize(Random.Range(0, _manager.Data.NumTotalCardKind));
            _cards[i].RegisterDeck(_manager);
        }
        _numCards = _size;
        _manager.HandAnalyser.UpdateHandData();
        GetRequiredCards();
    }
    public void Sort() { // Doesn't sort the last card
        for (int i = 1; i < _size-1; ++i) {
            Card key = _cards[i];
            int keyCode = (key == null) ? int.MaxValue : key.Code;
            int j = i-1;
            int targetCode = (_cards[j] == null) ? int.MaxValue : _cards[j].Code;
            while ((j >= 0) && (keyCode < targetCode)) {
                _cards[j+1] = _cards[j];
                j--;
                if (j < 0)
                    break;
                targetCode = (_cards[j] == null) ? int.MaxValue : _cards[j].Code;
            }
            _cards[j+1] = key;
        }
        AssignIndexToCards();
        Place();
    }
    public void AssignIndexToCards() {
        for (int i = 0 ; i < _size; ++i) {
            if (_cards[i] == null)
                continue;
            _cards[i].SetIndexInHand(i);
        }
    }
    public void Place() {
        float xPlacePosition = 0f;
        for (int i = 0; i < _size-1; ++i) {
            if (_cards[i] == null)
                continue;
            _cards[i].rectTransform.anchoredPosition = new Vector3(xPlacePosition, 0f, 0f);
            xPlacePosition += (_cards[i].rectTransform.rect.width < _cards[i].UISize.x) ? 0 : _cards[i].UISize.x;
        }
        if (_cards[_size-1] == null)
            return;
        xPlacePosition = _manager.RectTransform.rect.width - _cards[_size-1].rectTransform.rect.width;
        _cards[_size-1].rectTransform.anchoredPosition = new Vector3(xPlacePosition, 0f, 0f);
    }
    public void Draw() {
        if (_numCards >= _size)
            return;
        for (int i = _size-2; i >= 0; --i)
            if (_cards[i] == null) {
                _cards[i] = _cards[_size-1];
                break;
            }
        _cards[_size-1] = _manager.Bank.Withdraw();
        int luck = Random.Range(0, 3);
        int randomCode = (luck < 2) ? Random.Range(0, NumTotalCardKind) : _requiredCards[Random.Range(0, _requiredCards.Count)];
        _cards[_size-1].Initialize(randomCode);
        _numCards++;
        Sort();
        Place();
    }
    public void UseCard(int index) {
        if (index < 0 || index >= _size)
            return;

        if (_cards[index] is MergedCard head) {
            head = head.GetHead();
            for (MergedCard p = head; p != null; p = head) {
                head = head.next;
                _manager.MergedBank.Deposit(p);
                _cards[p.IndexInHand] = null;
                _numCards--;
            }
        }
        else {
            _manager.Bank.Deposit(_cards[index]);
            _cards[index] = null;
            _numCards--;
        }
        _cards[index] = _cards[^1];
        _cards[^1] = null;
        while (_numCards < _size) {
            _manager.Hand.Draw();
        }
        if (_manager.HuroController.IsHuroPrepared)
        {
            Debug.Log("JUST DO IT");
            _cards[^1].Initialize(_manager.HuroController.BonusCardCode);
            _manager.Merger.ToggleMergeMode();
            _manager.Merger.RegisterCandidate(_manager.HuroController.HurosHandIndice.Item1);
            _manager.Merger.RegisterCandidate(_manager.HuroController.HurosHandIndice.Item2);
            _manager.Merger.RegisterCandidate(_size - 1);
            _manager.Merger.ToggleMergeMode();
            _manager.HuroController.IsHuroPrepared = false;
        }
        Sort();

        _manager.HandAnalyser.UpdateHandData();
        int shanten = _manager.HandAnalyser.CalculateShanten();
        if (shanten == 0 && !_manager.RiichiController.IsRiichi) {
            _manager.RiichiController.SetRiichiableActive(true);
        }
        else if (shanten > 0) {
            _manager.RiichiController.SetRiichiableActive(false);
        }
        _manager.Cooldown.Activate();
        _manager.BonusCard.DrawNewBonusCard();
    }
    public void GetRequiredCards() {
        _requiredCards.Clear();
        _requiredCards = _manager.HandAnalyser.FindMachi();
    }

}
