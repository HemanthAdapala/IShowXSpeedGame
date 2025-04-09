using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Stack<IScreenBase> screenStack = new Stack<IScreenBase>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void NavigateTo(IScreenBase newScreen)
    {
        if (newScreen == null)
        {
            Debug.LogWarning("Trying to navigate to a null screen.");
            return;
        }

        // Don't push the same screen on top of itself
        if (screenStack.Count > 0 && screenStack.Peek() == newScreen)
        {
            Debug.Log("Trying to navigate to the screen already on top of the stack. Ignored.");
            return;
        }

        if (screenStack.Count > 0)
        {
            screenStack.Peek().Hide();
        }

        screenStack.Push(newScreen);
        PrintStack();
        newScreen.Show();
    }

    private void PrintStack()
    {
        Debug.Log("UI Stack:");
        foreach (var screen in screenStack)
        {
            Debug.Log($" - {screen}");
        }
    }


    public void GoBack()
    {
        if (screenStack.Count <= 1)
        {
            Debug.Log("Nothing to go back to.");
            return;
        }

        IScreenBase top = screenStack.Pop();
        PrintStack();
        top.Hide();

        IScreenBase previous = screenStack.Peek();
        previous.Show();
    }

    public IScreenBase GetCurrentScreen() => screenStack.Count > 0 ? screenStack.Peek() : null;
}
