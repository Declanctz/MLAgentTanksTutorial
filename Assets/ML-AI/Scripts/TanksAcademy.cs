using System.Collections;
using UnityEngine;
using MLAgents;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TanksAcademy : Academy
{

    [HideInInspector]
    public GameObject[] agents;

    public override void AcademyReset()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    void ClearObjects(GameObject[] objects)
    {

    }

    public override void AcademyStep()
    {

    }
}
