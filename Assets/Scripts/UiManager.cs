using TarodevController;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UiManager : MonoBehaviour
{
    [Header("Player Componets")]
    public GameObject Player;
    private PlayerController playerController;
    private MovementVariables moveVars;

    [Header("Key Componets")]
    public GameObject Key;
    private PickUpBehvaior pickUpBehvaior;

    [Header("Throwing Values")]
    public ThrowingVariables throwVars;

    [Header("Player Ui Elements")]
    public Canvas HorizontalBarCanvas;
    public TextMeshProUGUI HorizontalBarText;
    public TextMeshProUGUI keyTimerText;
    public float HorizontalBarCanvasOffset = 2;

    private float time;
    private float currentTimerTime;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        moveVars = Player.GetComponent<MovementVariables>();
        HorizontalBarText.gameObject.SetActive(false);

        pickUpBehvaior = Key.GetComponent<PickUpBehvaior>();

        HorizontalBarCanvas.gameObject.transform.position =
            new Vector3(Player.transform.position.x,
            Player.transform.position.y,
            Player.transform.position.z - HorizontalBarCanvasOffset);

        currentTimerTime = throwVars.keyResetTime;
        keyTimerText.text = currentTimerTime.ToString("F1");
        keyTimerText.gameObject.SetActive(false);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (pickUpBehvaior.hasBeenThrown)
        {
            keyTimerText.gameObject.SetActive(true);
            UpdateTimer();
        }

        if (playerController.GetPlayerState() == PlayerController.PlayerState.HorizontalBar)
        {
            HorizontalBarText.gameObject.SetActive(true);
            HorizontalBarText.text = playerController.getTimeUpHasBeenHeld().ToString("F1");
            HorizontalBarCanvas.gameObject.transform.position =
                new Vector3(Player.transform.position.x,
                Player.transform.position.y,
                Player.transform.position.z - HorizontalBarCanvasOffset);
        }
        else
        {
            HorizontalBarText.gameObject.SetActive(false);
        }

        if (playerController.GetPlayerState() == PlayerController.PlayerState.Carrying && pickUpBehvaior.beingCarried)
        {
            currentTimerTime = throwVars.keyResetTime;

            keyTimerText.gameObject.SetActive(false);
            
        }
    }

    private void UpdateTimer()
    {
        currentTimerTime -= Time.deltaTime;

        keyTimerText.gameObject.SetActive(true);

        keyTimerText.text = currentTimerTime.ToString("F1");

        if (currentTimerTime <= 0.0)
        {
            TimerEnded();
        }
    }

    private void TimerEnded()
    {
        Key.transform.position = pickUpBehvaior.initalPos;
        currentTimerTime = throwVars.keyResetTime;
        pickUpBehvaior.hasBeenThrown = false;
        keyTimerText.gameObject.SetActive(false);
    }
}
