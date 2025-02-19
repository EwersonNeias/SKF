using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;        // Referência ao UI Text para exibir o tempo
    public float startTime = 60f; // Tempo inicial do timer em segundos

    private float currentTime;    // Tempo atual do timer
    private bool isRunning = false; // Estado do timer (se está rodando ou não)

    void Start()
    {
        // Inicializa o timer com o tempo de início
        currentTime = startTime;
        UpdateTimerText();
        StartTimer(); // Inicia o timer automaticamente ao iniciar o jogo
    }

    void Update()
    {
        // Atualiza o timer a cada frame se ele estiver rodando
        if (isRunning)
        {
            currentTime -= UnityEngine.Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                KillAllPlayers();
            }
            UpdateTimerText();
        }
    }

    // Método para iniciar o timer
    public void StartTimer()
    {
        isRunning = true;
    }

    // Método para pausar o timer
    public void PauseTimer()
    {
        isRunning = false;
    }

    // Método para reiniciar o timer com o tempo inicial
    public void ResetTimer()
    {
        currentTime = startTime;
        UpdateTimerText();
        isRunning = false;
    }

    // Método para definir um novo tempo inicial e reiniciar o timer
    public void SetNewStartTime(float newStartTime)
    {
        startTime = newStartTime;
        ResetTimer();
    }

    // Atualiza o texto do UI com o tempo atual formatado como mm:ss
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Método para matar todos os objetos com a tag "Player"
    private void KillAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            LifeSystem lifeSystem = player.GetComponent<LifeSystem>();
            if (lifeSystem != null)
            {
                lifeSystem.Die();
            }
        }
    }
}
