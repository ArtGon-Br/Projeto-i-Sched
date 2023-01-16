using UnityEngine;

public class UIElement : MonoBehaviour
{
    Animator animator;

    bool isVisible = false;
    [SerializeField] bool enabledTimingToDisable;
    [SerializeField] float timeToDisable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (enabledTimingToDisable) Invoke("DisableObject", timeToDisable);
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

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
