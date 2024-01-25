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
    }
    protected void SetMagicImage() {
        switch (_attribute) {
            case ElementalAttribute.Pyro:
                _manager.Renderer.color = Color.red;
                break;
            case ElementalAttribute.Anemo:
                _manager.Renderer.color = Color.green;
                break;
            case ElementalAttribute.Hydro:
                _manager.Renderer.color = Color.blue;
                break;
            case ElementalAttribute.None:
                _manager.Renderer.color = Color.white;
                break;
            default:
                break;
        }
    }

}
