using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Projekt VR3
 * Kordian Kramek
 * Adrian Waler
 * Dawid Świerc
 * Dawid Piwko
 */

public class PlayerController : MonoBehaviour {

    public Rigidbody rb;
    public float speed;
    public float jumpSpeed;
    public int count;
    public int live;
    public Text countText;
    public Text levelText;
    public Text livesText;
    public float timeToReadText = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 10; 
        jumpSpeed = 10;

        count = 0;
        live = 5;

        levelText.text = "";
        SetScoreText();
        SetLivesText();
    }

    void BonusManager()
    {
        Bonus bonus = Bonus.GetBonus();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pk = GameObject.FindGameObjectsWithTag("PlayerKiller");
        GameObject[] pkBall = GameObject.FindGameObjectsWithTag("PkBall");
        GameObject[] pkHummer = GameObject.FindGameObjectsWithTag("PlayerKillerHummer");

        if (bonus.bonusName == "none")
        {
            ResetBonus();

        } else if(bonus.bonusName == "speed")
        {
            ResetBonus();
            speed = 30;
            player.GetComponent<Renderer>().material = Resources.Load("Materials/Speed") as Material;

        } else if(bonus.bonusName == "slow")
        {
            ResetBonus();
            speed = 3;
            player.GetComponent<Renderer>().material = Resources.Load("Materials/Slow") as Material;
        }
        else if (bonus.bonusName == "bullet time")
        {
            ResetBonus();
            Time.timeScale = 0.25f;
            speed = 30;
            player.GetComponent<Renderer>().material = Resources.Load("Materials/BulletTime") as Material;

        }
        else if (bonus.bonusName == "drunk")
        {
            ResetBonus();
            player.GetComponent<Renderer>().material = Resources.Load("Materials/Drunk") as Material;

        }
        else if (bonus.bonusName == "enemy")
        {
            ResetBonus();
            player.GetComponent<Renderer>().material = Resources.Load("Materials/Enemy") as Material;

            foreach (GameObject element in pk)
            {
                element.GetComponent<Renderer>().material = Resources.Load("Materials/Player") as Material;
            }

            foreach (GameObject element in pkBall)
            {
                element.GetComponent<Renderer>().material = Resources.Load("Materials/Player") as Material;
            }

            foreach (GameObject element in pkHummer)
            {
                element.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/Player") as Material;
            }

        }
        else if (bonus.bonusName == "invisible")
        {
            ResetBonus();
            player.GetComponent<Renderer>().material = Resources.Load("Materials/Invisible") as Material;
            Physics.IgnoreLayerCollision(9, 9, true);

        }
    }

    void ResetBonus()
    {
        speed = 10;
        Time.timeScale = 1;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] pk = GameObject.FindGameObjectsWithTag("PlayerKiller");
        GameObject[] pkBall = GameObject.FindGameObjectsWithTag("PkBall");
        GameObject[] pkHummer = GameObject.FindGameObjectsWithTag("PlayerKillerHummer");

        player.GetComponent<Renderer>().material = Resources.Load("Materials/Player") as Material;

        foreach (GameObject element in pk)
        {
            element.GetComponent<Renderer>().material = Resources.Load("Materials/PlayerKiller") as Material;
        }

        foreach (GameObject element in pkBall)
        {
            element.GetComponent<Renderer>().material = Resources.Load("Materials/PlayerKiller") as Material;
        }

        foreach (GameObject element in pkHummer)
        {
            element.GetComponentInChildren<Renderer>().material = Resources.Load("Materials/PlayerKiller") as Material;
        }

        Physics.IgnoreLayerCollision(9, 9, false);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        BonusManager();
        if (Bonus.GetBonus().bonusName == "drunk")
        {
            System.Random rnd = new System.Random();
            moveHorizontal += rnd.Next(-5, 5);
            moveVertical += rnd.Next(-5, 5);
        }

        //Ruszanie graczem
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * speed);

        //Skakanie używane jako "hack"
        if (Input.GetKeyDown("space"))
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            rb.AddForce(up * jumpSpeed, ForceMode.Impulse);
        }

        if(GameTime.GetGameTime().gameTimeLeft <= 0)
        {
            levelText.text = "GAME OVER!!!";
            Time.timeScale = 0; // Zatrzymujemy czas
        }

    }

    //Wykrywanie kolizji z elementami przez które się przechodzi (zaznaczone is kinematic)
    private void OnTriggerEnter(Collider other)
    {
        // Obiekty do zbierania
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false); // Ukrycie obiektu w przypadku kolizji
            count += 1; // Dodanie punktu
            SetScoreText(); // Zaktualizowanie wyniku
            GameTime.GetGameTime().gameTimeLeft += 5f;

            // Statyczne obiekty które trzeba omijać
        } else if (other.gameObject.CompareTag("PlayerKiller"))
        {
            if (Bonus.GetBonus().bonusName == "enemy")
            {
                other.gameObject.SetActive(false);
            } else
            {
                if(Bonus.GetBonus().bonusName != "invisible")
                {
                    live -= 1; // W przypadku kolizji odebranie punktu życia
                    SetLivesText(); //Zaktualizowanie punktów życia an UI
                }

            }

        }
        else if (other.gameObject.CompareTag("Drunk"))
        {
            Bonus.GetBonus().SetBonus("drunk");
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Bonus.GetBonus().SetBonus("enemy");
        }
        else if (other.gameObject.CompareTag("Invisible"))
        {
            Bonus.GetBonus().SetBonus("invisible");
        }
        else if (other.gameObject.CompareTag("Speed"))
        {
            Bonus.GetBonus().SetBonus("speed");
        }
        else if (other.gameObject.CompareTag("Slow"))
        {
            Bonus.GetBonus().SetBonus("slow");
        }
        else if (other.gameObject.CompareTag("BulletTime"))
        {
            Bonus.GetBonus().SetBonus("bullet time");
        }

    }

    //Wykrywanie kolizji z elementami przez które się nie przechodzi (odznaczone is kinematic)
    private void OnCollisionEnter(Collision collision)
    {
            if (collision.gameObject.CompareTag("PkBall") || collision.gameObject.CompareTag("PlayerKillerHummer"))
            {
                if (Bonus.GetBonus().bonusName != "invisible" && Bonus.GetBonus().bonusName != "enemy")
                {
                    live -= 1; // Odebranie punktu życia
                    SetLivesText(); // Zaktualizowanie punktów życia na UI 
                }
                else
                {
                    if(Bonus.GetBonus().bonusName == "enemy")
                    {
                        collision.gameObject.SetActive(false);

                    }
                    else if (Bonus.GetBonus().bonusName == "invisible")
                    {
                    
                        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), collision.collider);
                    }
                    
                }
    } 

    }

    // Aktualizacja punktów
    void SetScoreText()
    {
        // Aktualizacja UI
        countText.text = "Score: " + count.ToString();

        //W przypadku zebrania 13 pkt przechodzimy do następnego poziomu
        if (count == 13)
        {
            levelText.text = "Level 2"; //Wyświetlamy napis o przejściu do następnego poziomu
            GameObject lvl1Wall = GameObject.FindGameObjectWithTag("Lvl1Passed"); //Pobieramy obiekt ściany oddzielającej nas od poziomu drugiego
            lvl1Wall.SetActive(false); //Ukrywamy ścianę
            live += 1; // Dodajemy punkt życia

        }
        else if (count == 26)
        {
            levelText.text = "Level 3";
            GameObject lvl2Wall = GameObject.FindGameObjectWithTag("Lvl2Passed");
            lvl2Wall.SetActive(false);
            live += 1;

        }
        else if (count == 37)
        {
            levelText.text = "Level 4";
            GameObject lvl3Wall = GameObject.FindGameObjectWithTag("Lvl3Passed");
            lvl3Wall.SetActive(false);
            live += 1;

        }
        else if (count == 49)
        {
            levelText.text = "Level 5";
            GameObject lvl4Wall = GameObject.FindGameObjectWithTag("Lvl4Passed");
            lvl4Wall.SetActive(false);
            live += 1;

        }
        else if (count == 67)
        {
            levelText.text = "Level 6";
            GameObject lvl5Wall = GameObject.FindGameObjectWithTag("Lvl5Passed");
            lvl5Wall.SetActive(false);
            live += 1;

        }
        else if (count == 80)
        {
            levelText.text = "Level 67";
            GameObject lvl6Wall = GameObject.FindGameObjectWithTag("Lvl6Passed");
            lvl6Wall.SetActive(false);
            live += 1;

        }
        else if (count == 81)
        {
            levelText.text = "YOU WIN!!!"; // W przypadku zebrania wszystkich punktów wyświetlamy napis o wygranej

        }
        else
        {
            levelText.text = ""; // Czyścimy pole napisu środkowego
        }
    }

    //Ustawianie ilości żyć w UI
    void SetLivesText()
    {
        livesText.text = "Lives: " + live.ToString();

        // W przypadku ilości żyć mniejszej od 1 Wyświetlamy napis "GAME OVER" i zatrzymujemy czas
        if (live < 1)
        {
            levelText.text = "GAME OVER!!!";
            Time.timeScale = 0; // Zatrzymujemy czas
        }
    }

}
