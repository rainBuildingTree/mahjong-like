using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicModel : MonoBehaviour {
    public enum ElementalAttribute { Pyro, Anemo, Hydro, None, Error }
    protected ElementalAttribute _attribute;
    protected int _level;
    protected AttackManager _manager;

    public ElementalAttribute Attribute { get { return _attribute; } }
    public int Level { get { return _level; } }

    public void Init(ElementalAttribute attribute, int level) {
        _attribute = attribute;
        _level = level;
        _manager = GetComponent<AttackManager>();
        SetMagicImage();
        _manager.SetParticleMaterial(_attribute);
    }
    protected void SetMagicImage() {
        _manager.Renderer.sprite = _manager.SpriteStorage.GetSprite(_attribute);
    }

}
