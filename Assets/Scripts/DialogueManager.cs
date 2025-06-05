using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject acceptButton;
    [SerializeField] private GameObject rejectButton;

    private Queue<string> sentences;
    private Queue<string> names;

    private void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);
        StartDialogue();
    }

    public void StartDialogue()
    {
        EnqueueDialogue("Adam", "¿Qué… eres tú? ¿Qué demonios hago aquí?");
        EnqueueDialogue("Aztheroth", "No soy lo que ves. Ni tú lo eres aún. Si no me escuchas... morirás en esta cueva...");
        EnqueueDialogue("Adam", "Habla.");
        EnqueueDialogue("Aztheroth", "Tienes dos brazos y ninguno te sirve para salir...");
        EnqueueDialogue("Adam", "¿Qué me harás? ¿En qué me convertiré?");
        EnqueueDialogue("Aztheroth", "Una chispa. Si resistes, será tuyo...");
        EnqueueDialogue("Adam", "Hazlo. Pero si intentas algo más...");
        EnqueueDialogue("Aztheroth", "Ja ja ja eso... es lo más sensato que has dicho.");
        DisplayNextSentence();
    }

    private void EnqueueDialogue(string name, string sentence)
    {
        names.Enqueue(name);
        sentences.Enqueue(sentence);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            acceptButton.SetActive(true);
            rejectButton.SetActive(true);
            return;
        }

        nameText.text = names.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void AcceptDeal()
    {
        // Aquí puedes activar el brazo, cambiar animaciones, etc.
        Debug.Log("Jugador ha aceptado el trato con Aztheroth");
    }

    public void RejectDeal()
    {
        // Repetir la escena o mostrar otra reacción.
        Debug.Log("Jugador ha rechazado el trato. Reiniciando diálogo.");
        StartDialogue();
    }

}
