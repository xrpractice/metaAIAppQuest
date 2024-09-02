using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroqApiLibrary;
using Meta.WitAi.Dictation;
using Meta.WitAi.TTS.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LlamaController : MonoBehaviour
{
    private const string API_KEY = "your api key";

    private IGroqApiClient _groqApiClient ;
    string request ;
    [SerializeField]
    TMP_Text outputField;


    [SerializeField] private Toggle _toggle;

    [SerializeField]
    TMP_Text message;

    [SerializeField]
    TMP_InputField inputField;
    
    [SerializeField] private TTSSpeaker ttsSpeaker;
    
    [SerializeField] private DictationService _dictation;

    void Start()
    {
        _groqApiClient = new GroqApiClient(API_KEY);
        Debug.Log("Ayushi: groq connection established");
        // SendChatCompletionRequest();
    }

    public void ButtonOnclick()
    {
        SendChatCompletionRequest();
    }

    public async Task SendChatCompletionRequest()
    {
        if (_toggle.isOn)
        {
            request = message.text;
            request = Regex.Replace(request, @"\s+", " ");
            Debug.Log("Ayushi: voice to text:" + message.text);
        }
            
        else
            request = inputField.text;

        if (string.IsNullOrEmpty(request))
            request = "What is AI";
       
        Debug.Log("Ayushi: input: " + request);
        string result = await _groqApiClient.CreateChatCompletionAsync(request);
        Debug.Log("Ayushi: result: "+ result);
        outputField.text = result;
        PlayAudio(result);
    }
    
    public void ClearText()
    {
        message.text = "";
    }
    
    private void PlayAudio(string result)
    {
        if (_dictation.MicActive) 
            _dictation.Deactivate();
        ttsSpeaker.Speak(result);
    }
}