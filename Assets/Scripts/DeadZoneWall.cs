using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadZoneWall : MonoBehaviour
{

    PlayerController playerController;
    public Text scoreText;

    private void Awake() {
        string playerTagName = name.Replace("ScoreZoneFor", "");
        playerController = GameObject.Find(playerTagName).GetComponent<PlayerController>();
        scoreText = GameObject.Find($"Score{playerTagName}").GetComponent<Text>();
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D objectEntered)
    {
        if(objectEntered.CompareTag("Ball")) {
            playerController.IncreaseScore();
            scoreText.text = playerController.score.ToString();
            if(GameManager.sharedInstance.IsWinner(playerController.score)) {
                GameManager.winnerName = playerController.name;
                GameManager.sharedInstance.changeGameState(GameState.gameOver);
                return;
            } else {
                GameManager.sharedInstance.ResetMatch();
            }
        }
    }
}
