using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Tree : MonoBehaviour
{
    
    public DateTime dayPlant;             // The day the tree was planted
    public float collectionInterval = 5;    // The number of days until the item can be collected
    public Slider timerSlider;            // Reference to the UI slider element
    public Text timerText;                // Reference to the UI text element

    public Image GreenWhenDone;

    public GameObject EffectWhenDone;

    public GameObject collectSound;

    [SerializeField] private DateTime dayCollect;          // The day the item will be ready for collection
    private bool isCollectible = false;
    public TreeInformation treeInformation;
    public TreeInformation TreeInformation => treeInformation;

    void Start()
    {
        // Set the dayPlant to the current date if it's not set
        if (dayPlant == default)
        {
            dayPlant = DateTime.Now;
        }

        // Calculate the day the item will be ready for collection
        dayCollect = dayPlant.AddDays(collectionInterval);

        // Set the slider's max value to the total collection time in seconds
        timerSlider.maxValue = (float)(dayCollect - dayPlant).TotalSeconds;

        UpdateTimerUI();
        StartCoroutine(CheckCollectionReady());
    }

    void UpdateTimerUI()
    {
        TimeSpan timeRemaining = dayCollect - DateTime.Now;
        if (timeRemaining.TotalSeconds > 0)
        {
            timerText.text = $"{timeRemaining.Days}d {timeRemaining.Hours}h {timeRemaining.Minutes}m {timeRemaining.Seconds}s";
            timerSlider.value = (float)(collectionInterval * 86400 - timeRemaining.TotalSeconds);
        }
        else
        {
            timerText.text = "Collect!";
            timerSlider.value = timerSlider.maxValue;
            GreenWhenDone.color = new Color(95 / 255f, 172 / 255f, 62 / 255f, 255 / 255f);
        }
    }

    IEnumerator CheckCollectionReady()
    {
        while (!isCollectible)
        {
            TimeSpan timeRemaining = dayCollect - DateTime.Now;
            if (timeRemaining.TotalSeconds <= 0)
            {
                isCollectible = true;
                UpdateTimerUI();
                Debug.Log("Item is ready for collection!");
            }
            else
            {
                UpdateTimerUI();
            }
            yield return new WaitForSeconds(1); // Check every second
        }
    }

    public void CollectItem()
    {
        if (isCollectible)
        {
            // Add logic to give the item to the player
            Debug.Log("Item collected!");
            // Restart the collection timer
            dayPlant = DateTime.Now;
            dayCollect = dayPlant.AddDays(collectionInterval);
            isCollectible = false;
            StartCoroutine(CheckCollectionReady());
            // DATA xu ly o day
            treeInformation.Amount+=2;
            // collectSound.Play();
            GameObject obj = Instantiate(EffectWhenDone, transform.position, Quaternion.identity);
            GameObject collectObj = Instantiate(collectSound, transform.position, Quaternion.identity);

            Destroy(obj, 2f);
            Destroy(collectObj, 2f);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Item is not ready yet!");
        }
    }
}
