using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class TimerScript : MonoBehaviour
{
    public TMP_Text Timer;
    public float remainingTime;

    [SerializeField] private GameObject TimeUp;

    void Start()
    {
        if (TimeUp != null)
            TimeUp.SetActive(false);
    }
    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            Timer.text = Mathf.CeilToInt(remainingTime).ToString();
        }
        else if (remainingTime < 0f)
        {
            remainingTime = 0;
            Timer.text = Mathf.CeilToInt(remainingTime).ToString();
            if (TimeUp != null)
                TimeUp.SetActive(true);

            Time.timeScale = 0f;
        }
        
        if (remainingTime < 9f)
        {
            Timer.text = "0" + Mathf.CeilToInt(remainingTime).ToString(); 
        }
    }
}
