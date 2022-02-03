using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteColection", menuName = "MyAssets/SpriteColection")]
public class SpriteColection : ScriptableObject
{
    [SerializeField] private Sprite[] sprites;

    public Sprite[] Sprites
    {
        get { return sprites; }
    }
}
