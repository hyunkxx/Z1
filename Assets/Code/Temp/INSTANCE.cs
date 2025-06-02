using UnityEngine;

public class INSTANCE : MonoBehaviour
{
    public GameObject PC;
    public Character C;

    void Start()
    {
        GameObject obj = Instantiate<GameObject>(PC);
        PlayerController pc = obj.GetComponent<SurvivalController>();
        pc.ConnectCharacter(C);

        C.gameObject.AddComponent<CharacterAnimationController>();
    }
}
