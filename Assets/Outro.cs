using UnityEngine;

public class Outro : MonoBehaviour
{
    public void Onclick()
    {
        GameManager.Instance.returnToMainMenu();
    }
}
