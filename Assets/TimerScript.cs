using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class TimerScript : MonoBehaviour
{
    public TMP_Text Timer;
    public float remainingTime;

    // Update is called once per frame
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
        }
        
        if (remainingTime < 9f)
        {
            Timer.text = "0" + Mathf.CeilToInt(remainingTime).ToString(); 
        }
    }
}
