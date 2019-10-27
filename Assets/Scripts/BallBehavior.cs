using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    GameObject paddle;
    public Rigidbody2D ballRigidbody2D;

    AudioSource ballAudioSource;

    public static BallBehavior sharedInstance;
    float xPadding;

    void Awake() {
        ballAudioSource = GetComponent<AudioSource>();
        ballRigidbody2D = GetComponent<Rigidbody2D>();
        if(sharedInstance == null) {
            sharedInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        paddle = GameObject.Find("Player1");
        xPadding = paddle.transform.position.x - transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame) {
            transform.position = new Vector3(paddle.transform.position.x - xPadding, paddle.transform.position.y, transform.position.z);

            if(paddle.GetComponent<PlayerController>().DidPlayerTouch()) {
                GameManager.sharedInstance.changeGameState(GameState.matchStarted);
                int yDirection = Random.Range(0, 2) == 0 ? -1 : 1;
                ballRigidbody2D.velocity = new Vector2(8,8 * yDirection);
            }
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        ballAudioSource.Play();
    }
}
