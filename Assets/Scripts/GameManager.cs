using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    menu,
    inGame,
    matchStarted,
    gameOver
}

public enum Difficulty {
    easy,
    medium,
    hard
}


public class GameManager : MonoBehaviour
{
    public GameState currentGameState;

    public static GameManager sharedInstance;

    public static int winnerMaxScore = 3;
    public static string winnerName;

    public static int players = 1;

    public Difficulty difficulty;

    public const string IN_GAME_SCENE_NAME = "MatchScene", WINNER_SCENE_NAME = "WinScene",
                        LOSER_SCENE_NAME = "LoseScene", MENU_SCENE_NAME = "MenuScene";
    private void Awake() {
        if(sharedInstance == null) {
            sharedInstance = this;
        }
        switch (SceneManager.GetActiveScene().name)
        {
            case LOSER_SCENE_NAME:
            case WINNER_SCENE_NAME:
                currentGameState = GameState.gameOver;
                break;
            case IN_GAME_SCENE_NAME:
                currentGameState = GameState.inGame;
                break;
            case MENU_SCENE_NAME:
                currentGameState = GameState.menu;
                break;
        }

    }

    public void ResetMatch () {
        GameObject.Find("Player1").GetComponent<PlayerController>().ResetAttributes();
        changeGameState(GameState.inGame);
        winnerName = null;
    }

    public void changeGameState(GameState type) {
        switch (type)
        {
            case GameState.gameOver:
                string sceneToLoad = winnerName == "Player1" ? WINNER_SCENE_NAME : LOSER_SCENE_NAME;
                SceneManager.LoadScene(sceneToLoad);
                break;
            case GameState.matchStarted:
                break;
            case GameState.menu:
                players = 1;
                SceneManager.LoadScene(MENU_SCENE_NAME);
                break;
            case GameState.inGame:
                if(GameState.matchStarted != currentGameState) {
                    SceneManager.LoadScene(IN_GAME_SCENE_NAME);
                }
                break;
        }
        currentGameState = type;
    }

    public bool IsWinner(int currentScore) {
        return currentScore >= winnerMaxScore;
    }
    // OnClick Events
    public void ChangeGameStateToMenu() {
        changeGameState(GameState.menu);
    }

    public void ChangeGameStateToInGame() {
        changeGameState(GameState.inGame);
    }

    public void SetEasy(){
        difficulty = Difficulty.easy;
        ChangeGameStateToInGame();
    }

    public void SetMedium(){
        difficulty = Difficulty.medium;
        ChangeGameStateToInGame();
    }

    public void SetHard(){
        difficulty = Difficulty.hard;
        ChangeGameStateToInGame();
    }

    public void SetTwoPlayers() {
        players = 2;
        ChangeGameStateToInGame();
    }

}
