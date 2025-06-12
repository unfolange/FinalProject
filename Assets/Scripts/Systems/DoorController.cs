using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [Tooltip("¿Requiere llave para abrir?")]
    public bool needsKey = false;
    [Tooltip("Nombre de la escena a cargar al abrir")]
    public string nextSceneName;

    private bool isLocked = true;
    private Collider2D blockCollider;
    private Animator animator; // opcional
    private bool animationPlayed;

    void Awake()
    {
        blockCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>(); // si tienes animaciones
        animationPlayed = false;
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("Puerta abierta!");
        if (blockCollider != null) blockCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null) animator.SetTrigger("Open");
        LoadNextLevel();

        // var inv = other.GetComponent<PlayerInventory>();
        // if (needsKey && (inv == null || !inv.hasKey))
        // {
        //     UIManager.Instance.ShowMessage("Necesitas la llave para abrir.", 2f);
        //     return;
        // }

        // Ya abierto o no requiere llave
        // UIManager.Instance.ShowMessage("Pasando al siguiente nivel...", 1f);

    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
