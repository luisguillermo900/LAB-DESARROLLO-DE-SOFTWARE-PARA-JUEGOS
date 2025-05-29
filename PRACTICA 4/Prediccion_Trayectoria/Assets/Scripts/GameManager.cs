using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    Camera cam;

    public Ball ball;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;

    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        cam = Camera.main;
        ball.DesactivateRb();
        UpdateScoreText();
    }

    void Update()
    {
        // Control de arrastre
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnDragStart();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEnd();
        }

        if (isDragging)
        {
            OnDrag();
        }

        // Reinicio de bola al presionar ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetBall();
        }
    }

    void OnDragStart()
    {
        ball.DesactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        Debug.DrawLine(startPoint, endPoint);
        trajectory.UpdateDots(ball.pos, force);
    }

    void OnDragEnd()
    {
        ball.ActivateRb();
        ball.Push(force);
        trajectory.Hide();
    }

    // Método público para sumar puntos
    public void AddPoint()
    {
        score++;
        Debug.Log("Puntaje: " + score);
        UpdateScoreText();
    }

    // Actualiza el texto de puntaje en la UI
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + score;
        }
    }

    // Reinicia la bola a su posición inicial
    public void ResetBall()
    {
        ball.ResetPosition();
    }
}
