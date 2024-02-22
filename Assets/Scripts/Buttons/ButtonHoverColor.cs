using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverColor : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Color hoverColor;

    private Color originalColor;
    private ColorBlock cb;

    void Start()
    {
        cb = button.colors;
        originalColor = cb.selectedColor;
    }

    public void changeWhenHover() {
        cb.selectedColor = hoverColor;
        button.colors = cb;
    }

    public void changeWhenLeaves() {
        cb.selectedColor = originalColor;
        button.colors = cb;
    }
}
