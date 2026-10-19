using UnityEngine;

public class Finish : MonoBehaviour
{
    public Game game;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something hit Finish Line: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit Finish!");
            game.Finished();
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
