using UnityEngine;

public class UIElement : MonoBehaviour
{
    Animator animator;

    bool isVisible = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        if (animator == null) return;

        animator.SetTrigger("Show");
        animator.ResetTrigger("Hide");
        isVisible = true;
    }

    public void Hide()
    {
        if (animator == null) return;

        animator.SetTrigger("Hide");
        animator.ResetTrigger("Show");
        isVisible = false;
    }

    public void Toggle()
    {
        if (isVisible) Hide();
        else Show();
    }
}
