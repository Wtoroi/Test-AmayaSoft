using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogick : MonoBehaviour
{
    private Sprite sprite;
    private string name;
    private GameObject Image;
    private bool needRestart = false;

    public Sprite TargetSprite
    {
        set { sprite = value; }
    }

    public delegate void KlickOnTile(string Name, GameObject tile);
    public static event KlickOnTile Klicked;

    void Start()
    {
        TailSpawner.restart += Restart;

        name = sprite.name;

        Image = new GameObject();
        Image.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        Image.AddComponent<SpriteRenderer>().sprite = sprite;
        Image.transform.parent = gameObject.transform;
        Image.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);

        if (sprite.textureRect.width > sprite.textureRect.height + 80)
            Image.transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    private void Restart()
    {
        needRestart = true;
    }

    private void OnMouseDown()
    {
        if(!needRestart)
            Klicked(name, Image);
    }
}