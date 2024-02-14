using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDeckManager : MonoBehaviour {
    [SerializeField] protected DeckDataSO _data;
    [SerializeField] protected GameObject _cardPrefab;
    [SerializeField] protected GameObject _mergedCardPrefab;
    protected CardHand _hand;
    protected CardBank _bank;
    protected MergedCardBank _mergedBank;
    protected CardMerger _merger;
    protected RectTransform _rectTransform;
    protected AimController _aim;
    protected MergeButton _mergeButton;
    protected Cooldown _cooldown;
    protected HandAnalyzer _handAnalyzer;
    protected RiichiButton _riichiButton;
    protected SkipButton _skipButton;

    public CardHand Hand { get { return _hand; } }
    public CardBank Bank { get { return _bank; } }
    public MergedCardBank MergedBank { get { return _mergedBank; } }
    public DeckDataSO Data { get { return _data; } }
    public CardMerger Merger { get { return _merger; } }
    public RectTransform RectTransform { get { return _rectTransform; } }
    public AimController Aim { get { return _aim; } }
    public GameObject CardPrefab { get { return _cardPrefab; } }
    public GameObject MergedCardPrefab { get { return _mergedCardPrefab; } }
    public MergeButton MergeButton { get { return _mergeButton; } }
    public Cooldown Cooldown { get { return _cooldown; } }
    public HandAnalyzer HandAnalyzer { get { return _handAnalyzer; } }
    public RiichiButton RiichiButton { get { return _riichiButton; } }
    public SkipButton SkipButton { get { return _skipButton; } }

    protected void Awake() {
        _hand = gameObject.AddComponent<CardHand>();
        _bank = gameObject.AddComponent<CardBank>();
        _mergedBank = gameObject.AddComponent<MergedCardBank>();
        _merger = gameObject.AddComponent<CardMerger>();
        _rectTransform = GetComponent<RectTransform>();
        _aim = FindObjectOfType<AimController>();
        _mergeButton = FindObjectOfType<MergeButton>();
        _cooldown = GetComponentInChildren<Cooldown>();
        _handAnalyzer = gameObject.AddComponent<HandAnalyzer>();
        _riichiButton = FindObjectOfType<RiichiButton>();
        _skipButton = FindObjectOfType<SkipButton>();
    }
    protected void Start() {
        Init();
    }
    protected void Init() {
        _handAnalyzer.Init(this);
        _hand.Init();
        _bank.Init();
        _mergedBank.Init();
        _merger.Init();
        _riichiButton.gameObject.SetActive(false);
        _skipButton.gameObject.SetActive(false);
    }

}
