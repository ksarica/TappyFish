using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    private Rigidbody2D _rb;

    [SerializeField] private float _speed;

    int angle;
    int maxAngle = 20;
    int minAngle = -60;

    public Score score;
    bool touchedGround;

    public GameManager gameManager;
    public Sprite fishDied;
    SpriteRenderer sp;
    Animator anim;

    public ObstacleSpawner obstacleSpawner;

    [SerializeField] private AudioSource swim,hit,point;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        FishSwim();
    }

    private void FixedUpdate()
    {
        FishRotation();

    }

    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.gameOver == false)
        {
            swim.Play();
            if (GameManager.gameStarted == false)
            {
                _rb.gravityScale = 2;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed);
                obstacleSpawner.InstantiateObstacle();
                gameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, _speed); // yukarý doðru speed * F kuvvet uygulayacak
            }
        }
    }

    void FishRotation()
    {

        if (_rb.velocity.y > 0)
        {
            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }
        }
        else if (_rb.velocity.y < -1.2f)
        {
            if (angle > minAngle)
            {
                angle = angle - 2;
            }
        }
        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            score.Scored();
            point.Play();
        }
        else if (collision.CompareTag("Column") && GameManager.gameOver == false)
        {
            // game over
            gameManager.GameOver();
            FishDieEffect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // bu fonksiyonda yukarýdakinden farklý olarak .gameobject ekliyoruz ek olarak sebebi bilinmiyor
        {
            if (GameManager.gameOver == false)
            {
                // game over
                gameManager.GameOver();
                FishDieEffect();
                GameOver();

            }
            else
            {
                //game over (fish)
                GameOver();
            }
        }
    }

    private void FishDieEffect()
    {
        hit.Play();
    }
    private void GameOver()
    {
        touchedGround = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        sp.sprite = fishDied;

        anim.enabled = false;
    }
}


