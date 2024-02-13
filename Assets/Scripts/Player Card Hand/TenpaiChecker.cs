using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenpaiChecker : MonoBehaviour {
    private PlayerDeckManager _manager;
    int[] _pyros = {0,0,0,0,0,0,0,0,0};
    int[] _pyrosBackup = {0,0,0,0,0,0,0,0,0};
    int[] _anemos = {0,0,0,0,0,0,0,0,0};
    int[] _anemosBackup = {0,0,0,0,0,0,0,0,0};
    int[] _hydros = {0,0,0,0,0,0,0,0,0};
    int[] _hydrosBackup = {0,0,0,0,0,0,0,0,0};
    int[] _chars = {0,0,0,0,0,0,0};
    int[] _charsBackup = {0,0,0,0,0,0,0};
    int _numShanten = 8;
    int _numShantenBackup = 8;
    public void Init(PlayerDeckManager manager) {
        _manager = manager;
    }
    public int CalculateShanten()
    {
        // (!) Shanten calculation: 8 - (num completed body) * 2 - (num body candidate)

        // (0) Get the number of completed body
        for (int i = 0; i < _pyros.Length; ++i) {
            if (_pyros[i] > 2) {
                _pyros[i] -= 3;
                _numShanten -= 2;
            }
            if (_anemos[i] > 2) {
                _anemos[i] -= 3;
                _numShanten -= 2;
            }
            if (_hydros[i] > 2) {
                _hydros[i] -= 3;
                _numShanten -= 2;
            }
            if (i < _chars.Length && _chars[i] > 2) {
                _chars[i] -= 3;
                _numShanten -= 2;
            }
            if (i > 6)
                continue;
            if (_pyros[i] > 0 && _pyros[i+1] > 0 && _pyros[i+2] > 0) {
                _pyros[i]--;
                _pyros[i+1]--;
                _pyros[i+2]--;
                _numShanten -= 2;
            }
            if (_anemos[i] > 0 && _anemos[i+1] > 0 && _anemos[i+2] > 0) {
                _anemos[i]--;
                _anemos[i+1]--;
                _anemos[i+2]--;
                _numShanten -= 2;
            }
            if (_hydros[i] > 0 && _hydros[i+1] > 0 && _hydros[i+2] > 0) {
                _hydros[i]--;
                _hydros[i+1]--;
                _hydros[i+2]--;
                _numShanten -= 2;
            }   
        }
        // (1) Get the number of heads or candidates to be triplet
        bool isHeadPrepared = false;
        for (int i = 0; i < _pyros.Length; ++i) {
            if (_pyros[i] > 1) {
                _pyros[i] -= 2;
                _numShanten--;
                isHeadPrepared = true;
            }
            if (_anemos[i] > 1) {
                _anemos[i] -= 2;
                _numShanten--;
                isHeadPrepared = true;
            }
            if (_hydros[i] > 1) {
                _hydros[i] -= 2;
                _numShanten--;
                isHeadPrepared = true;
            }
            if (i < _chars.Length && _chars[i] > 1) {
                _chars[i] -= 2;
                _numShanten--;
                isHeadPrepared = true;
            }
        }
        if (_numShanten < 1)
            return _numShanten;
        // (2) Get the number of candidates to be sequence
        for (int i = 0; i < _pyros.Length-1; ++i) {
            if (_pyros[i] > 0) {
                if (_pyros[i+1] > 0) {
                    _pyros[i]--;
                    _pyros[i+1]--;
                    _numShanten--;
                }
                else if (i+2 < _pyros.Length && _pyros[i+2] > 0) {
                    _pyros[i]--;
                    _pyros[i+2]--;
                    _numShanten--;
                }
            }
            if (_anemos[i] > 0) {
                if (_anemos[i+1] > 0) {
                    _anemos[i]--;
                    _anemos[i+1]--;
                    _numShanten--;
                }
                else if (i+2 < _anemos.Length && _anemos[i+2] > 0) {
                    _anemos[i]--;
                    _anemos[i+2]--;
                    _numShanten--;
                }
            }
            if (_hydros[i] > 0) {
                if (_hydros[i+1] > 0) {
                    _hydros[i]--;
                    _hydros[i+1]--;
                    _numShanten--;
                }
                else if (i+2 < _hydros.Length && _hydros[i+2] > 0) {
                    _hydros[i]--;
                    _hydros[i+2]--;
                    _numShanten--;
                }
            }
        }
        _numShanten += isHeadPrepared ? 0 : 1;
        return _numShanten;
    }

    public void UpdateHandData()
    {
        Reset();
        Card[] cards = _manager.Hand.Cards;
        for (int i = 0; i < cards.Length-1; ++i)
        {
            if (cards[i] == null)
                continue;
            if (cards[i] is MergedCard) {
                _numShanten -= 2;
                _numShantenBackup -= 2;
                i += 2;
                continue;
            }
            switch (cards[i].Attribute) {
                case Card.ElementalAttribute.Pyro:
                    _pyros[cards[i].Number - 1]++;
                    _pyrosBackup[cards[i].Number - 1]++;
                    break;
                case Card.ElementalAttribute.Anemo:
                    _anemos[cards[i].Number - 1]++;
                    _anemosBackup[cards[i].Number - 1]++;
                    break;
                case Card.ElementalAttribute.Hydro:
                    _hydros[cards[i].Number - 1]++;
                    _hydrosBackup[cards[i].Number - 1]++;
                    break;
                case Card.ElementalAttribute.Char:
                    _chars[cards[i].Number - 1]++;
                    _charsBackup[cards[i].Number - 1]++;
                    break;
            }
        }
    }
    public List<int> FindWaits() {
        if (CalculateShanten() > 0)
            return null;
        List<int> waits = new List<int>();
        for (int i = 0; i < 9; ++i) {
            RestoreFromBackup();
            _pyros[i]++;
            if (CalculateShanten() < 0)
                waits.Add(i);
            RestoreFromBackup();
            _anemos[i]++;
            if (CalculateShanten() < 0)
                waits.Add(9 + i);
            RestoreFromBackup();
            _hydros[i]++;
            if (CalculateShanten() < 0)
                waits.Add(18 + i);
            if (i > 6)
                continue;
            RestoreFromBackup();
            _chars[i]++;
            if (CalculateShanten() < 0)
                waits.Add(27 + i);
        }
        return waits;
    }

    protected void RestoreFromBackup() {
        for (int i = 0; i < 9; ++i) {
            _pyros[i] = _pyrosBackup[i];
            _anemos[i] = _anemosBackup[i];
            _hydros[i] = _hydrosBackup[i];
            if (i < 7)
                _chars[i] = _charsBackup[i];
        }
        _numShanten = _numShantenBackup;
    }
    protected void Reset() {
        _numShanten = 8;
        _numShantenBackup = 8;
        for (int i = 0; i < _pyros.Length; ++i) {
            _pyros[i] = 0;
            _pyrosBackup[i] = 0;
            _anemos[i] = 0;
            _anemosBackup[i] = 0;
            _hydros[i] = 0;
            _hydrosBackup[i] = 0;
            if (i < _chars.Length) {
                _chars[i] = 0;
                _charsBackup[i] = 0;
            }
        }
    }
}
