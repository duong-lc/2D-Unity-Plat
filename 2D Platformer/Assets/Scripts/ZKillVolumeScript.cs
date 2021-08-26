using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZKillVolumeScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("enemies")){
            Destroy(other.gameObject);
        }
    }
}
