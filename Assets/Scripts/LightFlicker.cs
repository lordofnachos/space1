using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private float timer;
    void Update()
    {
        timer++;
        if(timer > 500)
        {
            timer = 0;
        }
    }
}
