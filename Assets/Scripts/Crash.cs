using UnityEngine;

public class Crash : MonoBehaviour
{
    public Game game;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something hit Obstacle: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit Obstacle!");
            game.Crashed();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
