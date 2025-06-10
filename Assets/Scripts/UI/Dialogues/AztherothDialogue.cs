using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class AztherothDialogue : MonoBehaviour
{
    [Header("Aztheroth")]
    [Tooltip("Assign the Animator component on your Aztheroth GameObject.")]
    public Animator aztherothAnimator;

    [Header("Dialogue")]
    public GameObject dialogueBox;
    public TMP_Text _text;
    public string[] lines;
    public float textSpeed;

    [Header("Timeline")]
    public PlayableDirector timeline;

    [Header("Brain (for cuts)")]
    public CinemachineBrain brain;

    [Header("Cameras")]
    public CinemachineCamera[] dialogueCameras;
    public int[] activeCamList; // Each entry should match a line index
    public CinemachineCamera fallbackCamera;

    [Header("Next Timeline (after dialogue)")]
    [Tooltip("Assign the PlayableDirector you want to play when dialogue ends.")]
    public PlayableDirector nextTimeline;
    [Header("Audio")]                                      // ► AUDIO
    public AudioSource audioSource;                         // ► AUDIO
    public AudioClip[] characterClips;  // tamaño 2: [0]=PersonajeA, [1]=PersonajeB  // ► AUDIO

    public AudioClip fallbackClip;

    private int index = 0;
    private bool dialoguing = false;

    void Start()
    {
        timeline.stopped += OnTimelineStopped;
        if (aztherothAnimator == null)
            Debug.LogWarning("aztherothAnimator no está asignado en el inspector.");
    }

    void OnDestroy()
    {
        timeline.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        brain.DefaultBlend = new CinemachineBlendDefinition(
            CinemachineBlendDefinition.Styles.Cut,
            0f
        );
        if (activeCamList != null && activeCamList.Length > index)
            UpdateActiveCamera(activeCamList[index]);
        StartCoroutine(StartDialogueAfterTimeline());
    }

    IEnumerator StartDialogueAfterTimeline()
    {
        yield return new WaitForSecondsRealtime(0.05f); // Let Timeline release camera control

        index = 0;
        dialoguing = true;
        // Time.timeScale = 0;
        _text.text = string.Empty;
        dialogueBox.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (!dialoguing) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                _text.text = lines[index];
            }
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            if (activeCamList != null && index < activeCamList.Length)
                UpdateActiveCamera(activeCamList[index]);
            else
                Debug.LogWarning($"No hay cámara asignada para la línea {index}");
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            _text.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }

    void UpdateActiveCamera(int activeIndex)
    {
        for (int i = 0; i < dialogueCameras.Length; i++)
        {
            dialogueCameras[i].Priority = (i == activeIndex) ? 100 : 0;
            aztherothAnimator.SetBool("Talking", activeCamList[index] == 1);
        }

        if (fallbackCamera != null)
            fallbackCamera.Priority = 0;

        if (audioSource != null
            && characterClips != null
            && activeIndex < characterClips.Length
            && characterClips[activeIndex] != null)
        {
            audioSource.clip = characterClips[activeIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void EndDialogue()
    {
        dialoguing = false;
        dialogueBox.SetActive(false);
        // Time.timeScale = 1;
        gameObject.SetActive(false);

        if (fallbackCamera != null)
        {
            fallbackCamera.Priority = 100;
            foreach (var cam in dialogueCameras)
                cam.Priority = 0;
        }
        audioSource.Stop();

        Input.ResetInputAxes();

        if (nextTimeline != null)
            nextTimeline.Play();
    }
}
