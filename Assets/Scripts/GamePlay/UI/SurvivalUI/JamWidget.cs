using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class JamWidget : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI jamText;
    public TextMeshProUGUI JamText => jamText;

    public Animator animator { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
