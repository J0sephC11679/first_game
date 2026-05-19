using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public Sprite[] characters; // Array that holds the character sprites
    public Image previewImage; // UI Image component to display the selected character

    private int currentIndex = 0; // Index to keep track of the currently selected character

    void Start()
    {
        UpdateCharacter(); //Runs when scene starts, makes sure the character appears in the preview image
    }

    public void NextCharacter()
    {
        currentIndex++; // Increments the index to select the next character
        if (currentIndex >= characters.Length) // If the index exceeds the array length, reset it to 0
        {
            currentIndex = 0;
        }
        UpdateCharacter(); // Updates the preview image with the new character
    }

    public void PreviousCharacter()
    {
        currentIndex--; // Decrements the index to select the previous character
        if (currentIndex < 0) // If the index goes below 0, set it to the last character in the array
        {
            currentIndex = characters.Length - 1;
        }
        UpdateCharacter(); // Updates the preview image with the new character
    }

    void UpdateCharacter()
    {
        previewImage.sprite = characters[currentIndex]; // Sets the preview image to the currently selected character sprite
    }

    public void StartGame()
    {
        GameData.selectedCharacter = currentIndex; // Saves the selected character index to the GameData class
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); // Loads the game scene
    }

}
