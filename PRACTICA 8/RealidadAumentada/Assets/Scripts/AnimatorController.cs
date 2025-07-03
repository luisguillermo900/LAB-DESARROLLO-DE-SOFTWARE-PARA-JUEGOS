using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    
    public string estado = "idle";

    // Lista de estados de animación
    private List<string> animationStates = new List<string>
    {
        "idle", 
        "walk", 
        "run", 
        "battleidle", 
        "attack1", 
        "attack2", 
        "attack3", 
        "hit", 
        "sturn", 
        "die"
    };

    // Índice del estado actual
    private int currentStateIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(estado);
    }

    // Método para ir al próximo estado
    public void NextAnimation()
    {
        // Incrementar el índice y hacer circular
        currentStateIndex = (currentStateIndex + 1) % animationStates.Count;
        
        // Reproducir el nuevo estado
        PlayCurrentState();
    }

    // Método para ir al estado previo
    public void PreviousAnimation()
    {
        // Decrementar el índice y hacer circular
        currentStateIndex = (currentStateIndex - 1 + animationStates.Count) % animationStates.Count;
        
        // Reproducir el nuevo estado
        PlayCurrentState();
    }

    // Método privado para reproducir el estado actual
    private void PlayCurrentState()
    {
        string currentState = animationStates[currentStateIndex];
        
        // Reproducir la animación
        animator.Play(currentState);
        
        // Log para depuración
        Debug.Log($"Reproduciendo animación: {currentState}");
    }

    // Método para obtener el nombre del estado actual
    public string GetCurrentStateName()
    {
        return animationStates[currentStateIndex];
    }

    // Método opcional para establecer un estado específico por índice
    public void SetAnimationState(int index)
    {
        if (index >= 0 && index < animationStates.Count)
        {
            currentStateIndex = index;
            PlayCurrentState();
        }
        else
        {
            Debug.LogWarning("Índice de animación fuera de rango");
        }
    }

}
