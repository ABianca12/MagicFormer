using TarodevController;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("Needed Componets")]
    public GameObject Player;
    public GameObject Key;
    private PlayerController playerController;
    private MovementVariables moveVars;

    [Header("Player Ui Elements")]
    public TextMeshProUGUI HorizontalBarText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        moveVars = Player.GetComponent<MovementVariables>();
        HorizontalBarText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.GetPlayerState() == PlayerController.PlayerState.HorizontalBar)
        {
            HorizontalBarText.gameObject.SetActive(true);
            HorizontalBarText.text = playerController.getTimeUpHasBeenHeld().ToString("F1");
        }
        else
        {
            HorizontalBarText.gameObject.SetActive(false);
        }
    }
}
