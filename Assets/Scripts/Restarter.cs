using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restarter : MonoBehaviour {

    public void Restart() {

        GameController.instance.RestartGame();
    }
}
