using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "death") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (collision.gameObject.tag == "finish") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 7) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void EndGame() {
        Application.Quit();
    }
}
