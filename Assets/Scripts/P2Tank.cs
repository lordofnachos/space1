using UnityEngine;
using UnityEngine.UI;

public class P2Tank : MonoBehaviour
{
    public Image blackCover;   // The UI Image that will act as the black cover

    private float timer;

    public float startFade;

    public float fadeSpeed;

    public float startTankBreak;

    private Animator p2TankAnim;

    private void Start()
    {
        p2TankAnim = GetComponent<Animator>();
        blackCover.color = new Color(8, 9, 44, 255);
    }

    private void Update()
    {
        timer = Time.time;

        if(timer >= startFade)
        {
            StartFade();
        }

        if(timer >= startTankBreak)
        {
            BreakTank();
        }
    }

    private void StartFade()
    {
        blackCover.color = new Color(8, 9, 44, blackCover.color.a - fadeSpeed);
    }

    private void BreakTank()
    {
        p2TankAnim.SetBool("break", true);
    }
}