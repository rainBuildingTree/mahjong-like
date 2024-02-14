using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnalyser : MonoBehaviour {
    private PlayerDeckManager _manager;
    private int _numMaxShanten = 8;
    private int[] _handData;
    private int[] _handDataBackup;

    public void Init(PlayerDeckManager manager) {
        _manager = manager;
        _handData = new int[_manager.Data.NumTotalCardKind];
        _handDataBackup =  new int[_manager.Data.NumTotalCardKind];
    }
    public int CalculateShanten()
    {
        int numCompletedBody = GetCompletedBodyCount();
        int numHead = GetHeadCount();
        int numBodyCandidate = GetBodyCandidateCount();

        // (!) Shanten calculation: 8 - (num completed body) * 2 - (num body candidate)
        if (numCompletedBody > 3)
            return _numMaxShanten - numCompletedBody * 2 - numHead;
        else if (numHead < 1)
            return _numMaxShanten - numCompletedBody * 2 - numBodyCandidate + 1;
        else
            return _numMaxShanten - numCompletedBody * 2 - numHead - numBodyCandidate;
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
                _numMaxShanten -= 2;
                i += 2;
                continue;
            }
            _handData[cards[i].Code]++;
            _handDataBackup[cards[i].Code]++;
        }
    }
    public List<int> FindMachi() {
        RestoreFromBackup();
        int currentShanten = CalculateShanten();
        List<int> machi = new List<int>();
        for (int i = 0; i < _handData.Length; ++i) {
            RestoreFromBackup();
            _handData[i]++;
            if (CalculateShanten() < currentShanten)
                machi.Add(i);
        }
        return machi;
    }



    private void RestoreFromBackup() {
        for (int i = 0; i < _handData.Length; ++i) {
            _handData[i] = _handDataBackup[i];
        }
    }
    private void Reset() {
        _numMaxShanten = 8;
        for (int i = 0; i < _handData.Length; ++i) {
            _handData[i] = 0;
            _handDataBackup[i] = 0;
        }
    }
    private int GetCompletedBodyCount() {
        int numCompletedBody = 0;
        int numCardKindPerAttribute = _manager.Data.NumCardKindPerAttribute;
        bool proceedFlag = false;

        for (int i = 0; i < _handData.Length; ++i) {
            do {
                if (_handData[i] > 2) {
                    _handData[i] -= 3;
                    numCompletedBody++;
                }
                else
                    proceedFlag = true;
            } while (!proceedFlag);
            proceedFlag = false;
            do {
                if (i / numCardKindPerAttribute != (i+2) / numCardKindPerAttribute)
                    break;
                if (i+2 >= _handData.Length)
                    break;
                if (_handData[i] > 0 && _handData[i+1] > 0 && _handData[i+2] > 0) {
                    _handData[i]--;
                    _handData[i+1]--;
                    _handData[i+2]--;
                    numCompletedBody++;
                }
                else
                    proceedFlag = true;
            } while (!proceedFlag);
            proceedFlag = false;
        }

        return numCompletedBody;
    }
    private int GetHeadCount() {
        int numHead = 0;
        bool proceedFlag = false;

        for (int i = 0; i < _handData.Length; ++i) {
            do {
                if (_handData[i] > 1) {
                    _handData[i] -= 2;
                    numHead++;
                }
                else
                    proceedFlag = true;
            } while (!proceedFlag);
            proceedFlag = false;
        }

        return numHead;
    }
    private int GetBodyCandidateCount() {
        int numBodyCandidate = 0;
        int numCardKindPerAttribute = _manager.Data.NumCardKindPerAttribute;
        bool proceedFlag = false;

        for (int i = 0; i < _handData.Length; ++i) {
            do {
                if (i / numCardKindPerAttribute != (i+1) / numCardKindPerAttribute)
                    break;
                if (i+1 >= _handData.Length)
                    break;
                if (_handData[i] > 0 && _handData[i+1] > 0) {
                    _handData[i]--;
                    _handData[i+1]--;
                    numBodyCandidate++;
                }
                else
                    proceedFlag = true;
            } while (!proceedFlag);
            proceedFlag = false;
            do {
                if (i / numCardKindPerAttribute != (i+2) / numCardKindPerAttribute)
                    break;
                if (i+2 >= _handData.Length)
                    break;
                if (_handData[i] > 0 && _handData[i+2] > 0) {
                    _handData[i]--;
                    _handData[i+2]--;
                    numBodyCandidate++;
                }
                else
                    proceedFlag = true;
            } while (!proceedFlag);
            proceedFlag = false;
        }

        return numBodyCandidate;
    }
}
