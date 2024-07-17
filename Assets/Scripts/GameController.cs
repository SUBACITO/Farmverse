using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject water; // Reference to the water GameObject

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Public method to start the WaterPlant coroutine
    public void StartWatering()
    {
        StartCoroutine(WaterPlant(water));
    }

    public IEnumerator WaterPlant(GameObject water)
    {
        if (!water.activeSelf)
        {
            water.SetActive(true);
            yield return new WaitForSeconds(5);
            water.SetActive(false);
        }
    }
}
