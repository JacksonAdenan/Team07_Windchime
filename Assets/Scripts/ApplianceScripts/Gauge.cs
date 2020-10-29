using UnityEngine;

public enum GaugeState
{ 
    LOW,
    MID,
    HIGH
}
public class Gauge : MonoBehaviour
{
    public GaugeState currentState;

    public float incrementAmount;
    public float decrementAmount;
    public float decrementSpeedSeconds;
    public float currentAmount;
    public float gaugeCap;

    float gaugeTimer;

    // Start is called before the first frame update
    void Start()
    {
        gaugeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gaugeTimer += Time.deltaTime;
        if (gaugeTimer >= decrementSpeedSeconds)
        {
            gaugeTimer = 0;
            GaugeDecrease();
        }

        if (currentAmount <= 25)
        {
            currentState = GaugeState.LOW;
        }
        if (currentAmount > 25 && currentAmount < 75)
        {
            currentState = GaugeState.MID;
        }
        if (currentAmount >= 75)
        {
            currentState = GaugeState.HIGH;
        }


    }

    public void Increase()
    {
        if ((currentAmount + incrementAmount) < gaugeCap)
        { 
            currentAmount += incrementAmount;
            Debug.Log("Gauge increased. Current amount: " + currentAmount);
        }
    }
    void GaugeDecrease()
    {
        if ((currentAmount - decrementAmount) > 0)
        { 
            currentAmount -= decrementAmount;
        }
    }

    public bool IsAboveHalf()
    {
        if (currentAmount > gaugeCap / 2)
        {
            return true;
        }
        return false;
    }

}
