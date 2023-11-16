using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{

    public void SwitchToImitation()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchToReinforcement()
    {
        SceneManager.LoadScene(0);
    }
}
