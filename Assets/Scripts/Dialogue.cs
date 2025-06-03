using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueBox; 
    public TMP_Text _text;
    public string[] lines;
    public float textSpeed;

    bool dialoguing = false;
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialoguing)
        {
            if(_text.text == lines[index])
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
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;

            Input.ResetInputAxes(); //Evita que el clic se "arrastre"
        }
    }

    void StartDialogue()
    {
        index = 0;
        Time.timeScale = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.gameObject.CompareTag("Player"))
        {            
            StartCoroutine(Co_StartDialogue());     
        }
    }

    IEnumerator Co_StartDialogue()
    {
        yield return new WaitForSeconds(0.02f);
        dialoguing = true;
        _text.text = string.Empty;
        dialogueBox.SetActive(true);
        StartDialogue();
    }

}
