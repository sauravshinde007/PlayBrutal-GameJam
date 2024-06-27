using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{

    [SerializeField] private string levelSelectScene = "LevelSelect";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(levelSelectScene);
        }
    }
}
