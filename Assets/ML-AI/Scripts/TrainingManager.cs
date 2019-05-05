using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Complete;

public class TrainingManager : MonoBehaviour
{

    public GameObject m_TankPrefab;
    public TankManager[] m_Tanks;

    // Use this for initialization
    private void Start()
    {
        // For all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }

    private void FixedUpdate()
    {
        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.GetComponent<TanksAgent>()._needReset)
            {
                // Manual Reset
                // Random Pos
                var pos = m_Tanks[i].m_SpawnPoint.position;
                pos.x += Random.value;
                pos.z += Random.value;
                m_Tanks[i].m_Instance.transform.position = pos;
                // Random Rot
                var rot = m_Tanks[i].m_SpawnPoint.rotation;
                rot *= Quaternion.AngleAxis(Random.value * 360, Vector3.up);
                m_Tanks[i].m_Instance.transform.rotation = rot;
                // Reset
                m_Tanks[i].m_Instance.GetComponent<TankHealthBrain>().enabled = false;
                m_Tanks[i].m_Instance.GetComponent<TankHealthBrain>().enabled = true;
                m_Tanks[i].m_Instance.GetComponent<Rigidbody>().isKinematic = true;
                m_Tanks[i].m_Instance.GetComponent<Rigidbody>().isKinematic = false;
                m_Tanks[i].m_Instance.GetComponent<TanksAgent>()._needReset = false;
            }
               
        }
    }
}
