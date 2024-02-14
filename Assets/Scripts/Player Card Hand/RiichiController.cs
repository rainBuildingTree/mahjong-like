using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiichiController : MonoBehaviour {
    private PlayerDeckManager _manager;
    private bool _isRiichi = false;
    private bool _isRiichiable = false;
    private Transform _target = null;

    public bool IsRiichiable { get { return _isRiichiable; } }
    public bool IsRiichi { get { return _isRiichi; } }

    public void Init() {
        _manager = GetComponentInParent<PlayerDeckManager>();
        _manager.SkipButton.onClick.AddListener(() => SetRiichiableActive(false));
        _manager.RiichiButton.onClick.AddListener(() => SetRiichiableActive(false));
        _manager.RiichiButton.onClick.AddListener(() => SetRiichiActive(true));
    }
    public void SetRiichiActive(bool isActive) {
        _isRiichi = isActive;

    }
    public void SetRiichiableActive(bool isActive) {
        _isRiichiable = isActive;
        _manager.SkipButton.gameObject.SetActive(isActive);
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
