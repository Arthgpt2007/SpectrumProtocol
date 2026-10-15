
using UnityEngine;
using System.Collections.Generic;

public class ReactorController : MonoBehaviour
{
    [Header("Reactor Nodes")]
    public ReactorNode[] nodes;

    [Header("Visuals")]
    public Renderer reactorRenderer;
    public GameObject victoryPanel;

    [Header("Finish Screen")]
    public FinishScreen finishScreen;

    private readonly List<int> selectedColors = new List<int>();
    private bool completed;

    public void SelectColor(int colorIndex)
    {
        if (completed) return;

        if (selectedColors.Contains(colorIndex))
        {
            selectedColors.Remove(colorIndex);
            UpdateNodes();
            return;
        }

        if (selectedColors.Count >= 3)
            return;

        selectedColors.Add(colorIndex);
        UpdateNodes();

        if (selectedColors.Count == 3)
            CheckCombination();
    }

    private void CheckCombination()
    {
        // Six-colour circular wheel
        for (int start = 0; start < 6; start++)
        {
            bool correct =
                selectedColors.Contains(start) &&
                selectedColors.Contains((start + 1) % 6) &&
                selectedColors.Contains((start + 2) % 6);

            if (correct)
            {
                CompleteReactor();
                return;
            }
        }

        Debug.Log("Incorrect analogous combination!");
        // Leave selections active so the player can deselect
        // and try another combination.
    }

    private void UpdateNodes()
    {
        foreach (ReactorNode node in nodes)
        {
            if (node != null)
                node.SetSelected(
                    selectedColors.Contains(node.colorIndex)
                );
        }
    }


    private void CompleteReactor()
    {
        completed = true;

        if (reactorRenderer != null)
            reactorRenderer.material.color = Color.green;

        Debug.Log("SPECTRUM RESTORED!");

        if (finishScreen != null)
        {
            finishScreen.ShowFinishScreen();
        }
        else
        {
            Debug.LogError("FinishScreen not assigned!");
        }
    }

}
