using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TailSpawner : MonoBehaviour
{
    [SerializeField] private LevelSetings[] levelSetings;
    [SerializeField] private SpriteColection[] spriteColections;
    [SerializeField] private GameObject RestartSkreen;
    [SerializeField] private GameObject LoadSkreen;

    private int Iterator;
    private const int TileSkale = 2;
    private List<string> curentList = new List<string>();
    private Sequence sequence;
    private string lastTaret = "";

    public delegate void GenFinished(string nameTarget, int Iterator);
    public static event GenFinished Finish;

    public delegate void NeedRestart();
    public static event NeedRestart restart;

    public void Restart()
    {
        RestartSkreen.active = false;
        LoadSkreen.active = true;

        sequence = DOTween.Sequence();
        Image image = LoadSkreen.transform.GetComponent<Image>();
        sequence.Append(image.DOFade(0, 0));
        sequence.Append(image.DOFade(1, 2));

        StartCoroutine(restartWaiter());
    }
    IEnumerator restartWaiter()
    {
        yield return new WaitForSeconds(3);
        CleerSkreen();
        LoadSkreen.active = false;
        GenLevel();
    }

    void Start()
    {
        LevelOverseer.Win += NextLevel;
        Iterator = 0;
        GenLevel();
    }

    private void NextLevel()
    {
        Iterator++;
        if(Iterator < 3)
        {
            GenLevel();
            return;
        }

        restart();
        RestartSkreen.active = true;
        Image image = RestartSkreen.transform.GetChild(0).GetComponent<Image>();
        sequence.Append(image.DOFade(0, 0));
        sequence.Append(image.DOFade(0.5f, 0.5f));
        Iterator = 0;
    }

    private void CleerSkreen()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        curentList = new List<string>();
    }

    private void GenLevel()
    {
        CleerSkreen();

        float StartXpos = levelSetings[Iterator].Column / 2;
        float StartYpos = levelSetings[Iterator].Lines / 2;
        int NumberColection = Random.Range(0, spriteColections.Length);

        for (int x = 0; x < levelSetings[Iterator].Column; x++)
        {
            for (int y = 0; y < levelSetings[Iterator].Lines; y++)
            {
                Sprite currentSprite;
                while (true)
                {
                    int NumberSprite = Random.Range(0, spriteColections[NumberColection].Sprites.Length);
                    currentSprite = spriteColections[NumberColection].Sprites[NumberSprite];
                    if (!curentList.Contains(currentSprite.name))
                    {
                        curentList.Add(currentSprite.name);
                        break;
                    }
                }

                GameObject Tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Tile.transform.localScale = new Vector3(TileSkale, TileSkale, 1);
                Tile.AddComponent<Rigidbody>();
                Tile.AddComponent<TileLogick>().TargetSprite = currentSprite;
                Tile.GetComponent<MeshCollider>().convex = true;
                Tile.transform.position = new Vector3((x - StartXpos) * TileSkale, (y - StartYpos) * TileSkale, 1);
                Tile.transform.parent = gameObject.transform;
            }
        }

        string currentTaret;
        if (lastTaret == "")
        {
            lastTaret = curentList[Random.Range(0, curentList.Count)];
        }
        else
        {
            while (true)
            {
                currentTaret = curentList[Random.Range(0, curentList.Count)];
                if (currentTaret != lastTaret)
                {
                    lastTaret = currentTaret;
                    break;
                }
            }
        }
        Finish(lastTaret, Iterator);

        if (Iterator == 0)
        {
            foreach (Transform child in transform)
            {
                sequence = DOTween.Sequence();
                sequence.Append(child.transform.DOScale(0.01f, 0));
                sequence.Append(child.transform.DOScale(1f, 0.5f));
                sequence.Append(child.transform.DOScale(0.5f, 0.5f));
                sequence.Append(child.transform.DOScale(TileSkale / 2, 0.5f));
                sequence.Append(child.transform.DOScale(0.5f, 0.5f));
                sequence.Append(child.transform.DOScale(TileSkale, 0.5f));
            }
        }
    }
}