using UnityEngine;

public class Gauge : MonoBehaviour
{

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
        if (gaugeTimer == decrementSpeedSeconds)
        {
            gaugeTimer = 0;
            GaugeDecrease();
        }
    }

    void Increase()
    {
        currentAmount += incrementAmount;
    }
    void GaugeDecrease()
    {
        currentAmount -= decrementAmount;
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
