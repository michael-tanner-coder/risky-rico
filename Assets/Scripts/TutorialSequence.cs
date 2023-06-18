using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private List<TutorialPrompt> _prompts = new List<TutorialPrompt>();
    [SerializeField] private TMP_Text _promptText;
    [SerializeField] private TMP_Text _promptCountText;
    [SerializeField] private GameObject _floatTextPrefab;

    private int _inputCounter = 0;
    private int _promptIndex = 0;

    void Awake()
    {
        _promptText.text = CurrentPrompt().PromptText;
        _promptCountText.text = 0 + "/" + CurrentPrompt().MaxInputCounter;
    }

    void Update()
    {
        TutorialPrompt prompt = CurrentPrompt();

        if (_playerInput.actions[prompt.InputKey].triggered && _inputCounter < prompt.MaxInputCounter)
        {
            // update the text information and play a success sound
            _inputCounter++;        
            _promptCountText.text = _inputCounter + "/" + prompt.MaxInputCounter;
            ServiceLocator.Instance.Get<AudioManager>().PlaySoundFromDictionary("BonusPoints");

            // spawn text object to show a success message
            GameObject floatText = Instantiate(_floatTextPrefab, transform.position, transform.rotation);
            floatText.GetComponent<TMP_Text>().text = "" + _inputCounter + "/" + prompt.MaxInputCounter + "!";
            floatText.transform.SetParent(transform.parent);
        }

        if (_inputCounter >= prompt.MaxInputCounter)
        {
            GoToNextPrompt();
        }
    }

    void GoToNextPrompt()
    {
        _promptIndex++;
        _inputCounter = 0;

        if (_promptIndex <= _prompts.Count - 1)
        {
            _promptText.text = CurrentPrompt().PromptText;
            _promptCountText.text = 0 + "/" + CurrentPrompt().MaxInputCounter;
            return;   
        }

        ClosePromptSequence();
    }

    void ClosePromptSequence()
    {
        gameObject.SetActive(false);
    }

    public TutorialPrompt CurrentPrompt()
    {
        return _prompts[_promptIndex];
    }
}
