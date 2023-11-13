using System.Collections;
using UnityEngine;
using TMPro;

public class Conversation : MonoBehaviour
{
    [SerializeField] private string[] phrases = { "Hey, this is the shop!", "Please, choose your next weapon!", "Let the carnage begin!" };
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float typingSpeed;

    //private int currentPhraseIndex;
    //private bool isTyping;
    //private bool inTriggerZone;
    private int currentIndex = 0;
    private bool animateText = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Time.timeScale != 0)
        {
            StartCoroutine(AnimateText());
        }
    }
    
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Time.timeScale != 0)
        {
            StopCoroutine(AnimateText());
        }
    }
    
    private IEnumerator AnimateText()
    {
        while (animateText)
        {
            string currentPhrase = phrases[currentIndex];
    
            textMeshPro.text = "";
    
            for (int i = 0; i < currentPhrase.Length; i++)
            {
                textMeshPro.text += currentPhrase[i];
    
                yield return new WaitForSeconds(typingSpeed);
            }
    
            yield return new WaitForSeconds(2f);
    
            currentIndex = (currentIndex + 1) % phrases.Length;
        }
    }
    //private void Start()
    //{
    //    textMeshPro.text = "";
    //}
    //
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && Time.timeScale != 0)
    //    {
    //        inTriggerZone = true;
    //        StartConversation();
    //    }
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player") && Time.timeScale != 0)
    //    {
    //        inTriggerZone = false;
    //        StopConversation();
    //    }
    //}
    //
    //private void StartConversation()
    //{
    //    currentPhraseIndex = 0;
    //    isTyping = true;
    //    textMeshPro.text = "";
    //    StartCoroutine(TypeText(phrases[currentPhraseIndex]));
    //}
    //
    //private void StopConversation()
    //{
    //    isTyping = false;
    //    textMeshPro.text = "";
    //}
    //
    //IEnumerator TypeText(string phrase)
    //{
    //    foreach (char letter in phrase)
    //    {
    //        textMeshPro.text += letter;
    //        yield return new WaitForSeconds(typingSpeed);
    //    }
    //
    //    yield return new WaitForSeconds(1f);
    //
    //    textMeshPro.text = "";
    //
    //    if (currentPhraseIndex < phrases.Length - 1)
    //    {
    //        currentPhraseIndex++;
    //        StartCoroutine(TypeText(phrases[currentPhraseIndex]));
    //    }
    //}
}