using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quite : MonoBehaviour
{
    // Start is called before the first frame update
    public void Quit()
    {
        Debug.Log("Quitgame");
        Application.Quit();
    }
}
