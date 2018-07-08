using UnityEngine;
using UnityEngine.UI;

class GameTime : MonoBehaviour
{
    private static GameTime _gameTime;
    public Text timeText;
    public float gameTimeLeft = 100.0f;
    Text text;

    public GameTime()
    {
    }

    void Start()
    {
        _gameTime = this;
    }

    public static GameTime GetGameTime()
    {
        return _gameTime;
    }

    void Update()
    {
        gameTimeLeft -= Time.deltaTime;
        SetTimeText();

        if (gameTimeLeft < 0)
        {
            //levelText.text = "GAME OVER!!!";
            //Time.timeScale = 0; // Zatrzymujemy czas
        }
    }

    //Ustawianie ilości żyć w UI
    void SetTimeText()
    {
        timeText.text = "Time: " + gameTimeLeft.ToString();
    }

}
