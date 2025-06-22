using MainCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data
{
    public class Initialize : MonoBehaviour
    {
        [SerializeField, Tooltip("The room to start in.")]
        private RoomData startingRoom;

        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene("AdditiveGameplay", LoadSceneMode.Single);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}