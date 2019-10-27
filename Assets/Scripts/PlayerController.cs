using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   SpriteRenderer playerSpriteRender;

   public bool isBot = false;
   public float botSpeed = .1f;
    //The Color to be assigned to the Renderer’s Material

    public int score {get; private set;}

    // Variables for mobile
    float playerYCoordinateTouched;
    Vector3 mouseCoordinateTouched;

    int touched = -1;

    void Start()
    {
        playerSpriteRender = GetComponent<SpriteRenderer>();
        if(isBot) {
            if(GameManager.players == 2) {
                isBot = false;
                return;
            }

            switch (GameManager.sharedInstance.difficulty)
            {
                case Difficulty.medium:
                    botSpeed *= 1.2f;
                    break;
                case Difficulty.hard:
                    botSpeed *= 1.4f;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(isBot) {
            LimitPlayerBoundariesBot();
            AIBot();
        } else {
            LimitPlayerBoundariesPlayer();
        }

    }

    void AIBot() {
        if (GameManager.sharedInstance.currentGameState == GameState.matchStarted)
        {
            BallBehavior ball = BallBehavior.sharedInstance;

            float speed = ball.ballRigidbody2D.velocity.x < 0 ? botSpeed * Random.Range(0.55f, 0.9f) : botSpeed;

            if(transform.position.y < ball.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            }else if (transform.position.y > ball.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
            }
        }
    }

    void LimitPlayerBoundariesPlayer() {

        // Mobile functionality
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            foreach (var touch in Input.touches)
            {
                if(TouchPhase.Ended == touch.phase && touch.fingerId == touched) {
                    touched = -1;
                }

                if(TouchPhase.Began == touch.phase) {
                    Vector3 tmpPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    if( IsInGameZone(tmpPoint.x)){
                        playerYCoordinateTouched = transform.position.y;
                        mouseCoordinateTouched = tmpPoint;
                        touched = touch.fingerId;
                        Debug.Log(touched);
                    }
                }

                if(touch.fingerId == touched) {
                    if(Input.GetButton("Fire1") && IsInGameZone(mouseCoordinateTouched.x)) {
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(transform.position.x, Mathf.Clamp(playerYCoordinateTouched + (mousePosition.y - mouseCoordinateTouched.y ), LimitDown(), LimitUp()), transform.position.z);
                    }
                }
            }
        } else {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(mousePosition.y, LimitDown(), LimitUp()), transform.position.z);
        }
    }

    bool IsInGameZone (float xCoordinate) {
        return name == "Player1" ? (xCoordinate <= 0) : (xCoordinate >= 0);
    }

    void LimitPlayerBoundariesBot() {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, LimitDown(), LimitUp()), transform.position.z);
    }

    float LimitDown()
    {
        // Extents get distance from the center of the sprite
        return CameraOrigin().y + playerSpriteRender.bounds.extents.y;
    }

    float LimitUp()
    {
        return CameraBounds().y - playerSpriteRender.bounds.extents.y;
    }

    Vector2 CameraBounds()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    Vector2 CameraOrigin()
    {
        return Camera.main.ScreenToWorldPoint(Vector2.zero);
    }

    public void IncreaseScore()
    {
        score++;
    }

    public bool DidPlayerTouch() {
        return touched != -1;
    }

    public void ResetAttributes() {
        touched = -1;
    }
}
