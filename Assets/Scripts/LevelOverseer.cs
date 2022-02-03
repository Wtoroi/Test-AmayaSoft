using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelOverseer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taretText;
    [SerializeField] private GameObject particleEmitter;

    private string target;
    private Sequence sequence;

    public delegate void WinAlert();
    public static event WinAlert Win;

    private void Start()
    {
        TailSpawner.Finish += setTarget;
        TileLogick.Klicked += testFinish;
    }

    private void setTarget(string targetName, int Iterator)
    {
        target = targetName;
        taretText.text = "Find - " + target;

        if(Iterator == 0)
        {
            sequence = DOTween.Sequence();
            sequence.Append(taretText.DOFade(0, 0));
            sequence.Append(taretText.DOFade(1, 3));
        }
    }

    private void testFinish(string testName, GameObject Tile)
    {
        if (testName == target)
        {
            particleEmitter.transform.position = Tile.transform.position;
            particleEmitter.GetComponent<ParticleSystem>().Play();
            sequence = DOTween.Sequence();
            sequence.Append(Tile.transform.DOScale(0.3f, 0.4f));
            sequence.Append(Tile.transform.DOScale(0.15f, 0.5f));
            sequence.Append(Tile.transform.DOScale(0.3f, 0.4f));
            sequence.Append(Tile.transform.DOScale(0.15f, 0.5f));
            StartCoroutine(winWaiter());
        }
        else
        {
            sequence = DOTween.Sequence();
            sequence.Append(Tile.transform.DOShakePosition(1, 0.2f, 10, 90, false, true));
        }
    }

    IEnumerator winWaiter()
    {
        yield return new WaitForSeconds(2);
        Win();
    }
}
