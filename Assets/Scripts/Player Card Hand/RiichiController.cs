using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RiichiController : MonoBehaviour {
    private PlayerDeckManager _manager;
    private bool _isRiichi = false;
    private bool _isRiichiable = false;
    private bool _isRiichiPrepared = false;
    private Transform _target = null;

    public bool IsRiichiable { get { return _isRiichiable; } }
    public bool IsRiichi { get { return _isRiichi; } }

    public void Init() {
        _manager = GetComponentInParent<PlayerDeckManager>();
        _manager.RiichiButton.onClick.AddListener(() => ToggleRiichiPreparation());
    }
    public void StartRiichi() {
        if (!_isRiichiPrepared)
            return;
        _isRiichi = true;
        _isRiichiPrepared = false;
        _isRiichiable = false;
        _manager.RiichiButton.gameObject.SetActive(false);
        Card[] cards = _manager.Hand.Cards;
        for (int i = 0; i < _manager.Hand.Size; ++i) {
            if (cards[i] == null) continue;
            cards[i].SetScreenerActive(false);
        }
    }
    public void ToggleRiichiPreparation() {
        Card[] cards = _manager.Hand.Cards;
        if (_isRiichiPrepared) {
            _isRiichiPrepared = false;
            for (int i = 0; i < _manager.Hand.Size; ++i) {
                if (cards[i] == null) continue;
                cards[i].SetScreenerActive(false);
            }
            return;
        }
        List<int> cardIndiceToThrow = _manager.HandAnalyser.FindCardsToThrow();
        
        for (int i = 0; i < _manager.Hand.Size; ++i) {
            if (cardIndiceToThrow.Contains(i)) continue;
            if (cards[i] == null) continue;
            cards[i].SetScreenerActive(true);
        }
        _isRiichiPrepared = true;
    }
    public void SetRiichiableActive(bool isActive) {
        _isRiichiable = isActive;
        _manager.RiichiButton.gameObject.SetActive(isActive);
    }
    public void ProceedRiichi() {
        if (!_isRiichi) return;
        _target = _manager.EnemyPool.GetEnemyInAttribute(_manager.Hand.Cards[^1].Attribute);
        if (_target == null)
            _target = _manager.EnemyPool.ClosestEnemy(new Vector2(100f, 0f));
        _manager.AttackPool.EnableObject((MagicModel.ElementalAttribute)_manager.Hand.Cards[^1].Attribute, 1, _target);
        _manager.Hand.UseCard(_manager.Hand.Size - 1);
    }
}
