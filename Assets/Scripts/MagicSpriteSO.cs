using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MagicSpriteSO : ScriptableObject {
    public Sprite[] sprites;
    public Material[] materials;

    public Sprite GetSprite(MagicModel.ElementalAttribute attribute) {
        switch (attribute) {
            case MagicModel.ElementalAttribute.Pyro:
                return sprites[0];
            case MagicModel.ElementalAttribute.Anemo:
                return sprites[1];
            case MagicModel.ElementalAttribute.Hydro:
                return sprites[2];
            case MagicModel.ElementalAttribute.None:
                return sprites[3];
            default:
                return null;
        }
    }
    public Material GetMaterial(MagicModel.ElementalAttribute attribute) {
        switch (attribute) {
            case MagicModel.ElementalAttribute.Pyro:
                return materials[0];
            case MagicModel.ElementalAttribute.Anemo:
                return materials[1];
            case MagicModel.ElementalAttribute.Hydro:
                return materials[2];
            case MagicModel.ElementalAttribute.None:
                return materials[3];
            default:
                return null;
        }
    }
}
