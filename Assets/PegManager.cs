using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class PegManager : MonoBehaviour
{
    bool _isSimulation = false;
    private Random _random;
    [SerializeField] private bool doRandomize = false;
    [SerializeField] private int[] pegRange = new int[]{10,10};
    [SerializeField] private int pegCount = 10;
    [SerializeField] private GameObject pegPrefab;
    private List<Transform> _pegs = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        _random = GameManager.Instance.GetRandom();
        _isSimulation = gameObject.tag.Equals("Simulated");
        
        if (doRandomize)
        {
            for (int i = 0; i < pegCount; i++)
            {
                _pegs.Add(Instantiate(pegPrefab, transform.position, Quaternion.identity, transform).transform);
            }
            RandomizeAndSyncPegs();
        }
    }

    private void RandomizeAndSyncPegs()
    {
        if (!_isSimulation)
        {
            foreach (Transform child in transform)
            {
                int randomX = _random.NextInt(pegRange[0]);
                int randomY = _random.NextInt(pegRange[1]);
                child.localPosition = new Vector3(randomX, randomY, 0);
            }
        }
        else
        {
            SyncPegs();
        }
    }

    private void SyncPegs()
    {
        Transform[] realPegs = GameManager.Instance.pegManager.GetPegs().ToArray();
        if (realPegs.Length > transform.childCount)
        {
            for (int i = 0; i < realPegs.Length - transform.childCount; i++)
            {
                Instantiate(pegPrefab, transform.position, Quaternion.identity, transform);
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Renderer>().enabled = false;
            child.transform.position = realPegs[i].position;
        }
    }

    public void Reset(bool won)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<PegScript>().Reset(_isSimulation);
        }

        if (doRandomize && won)
        {
            RandomizeAndSyncPegs();
        }
    }
    
    public List<Transform> GetPegs()
    {
        return _pegs;
    }
}