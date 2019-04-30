using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Complete;

public class TrainingManager : MonoBehaviour
{

    public GameObject m_TankPrefab;
    public TankManager[] m_Tanks;

    // Use this for initialization
    void Start()
    {
        SpawnAllTanks();

        StartCoroutine(GameLoop());
    }

    private void SpawnAllTanks()
    {
        // For all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        ResetAllTanks();

        yield return StartCoroutine(RoundPlaying());

        StartCoroutine(GameLoop());
    }


    private IEnumerator RoundPlaying()
    {
        // While there is not one tank left...
        while (!OneTankDead())
        {
            // ... return on the next frame.
            yield return new WaitForFixedUpdate();
        }
    }

    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OneTankDead()
    {
        // Start the count of tanks dead at zero.
        int numTanksDead = 0;

        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.GetComponent<TankHealthBrain>().Dead)
                numTanksDead++;
        }

        return numTanksDead > 0;
    }

    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // Manual Reset
            m_Tanks[i].m_Instance.transform.position = m_Tanks[i].m_SpawnPoint.position;
            m_Tanks[i].m_Instance.transform.rotation = m_Tanks[i].m_SpawnPoint.rotation;
            m_Tanks[i].m_Instance.GetComponent<TankHealthBrain>().enabled = false;
            m_Tanks[i].m_Instance.GetComponent<TankHealthBrain>().enabled = true;
            m_Tanks[i].m_Instance.GetComponent<Rigidbody>().isKinematic = true;
            m_Tanks[i].m_Instance.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
