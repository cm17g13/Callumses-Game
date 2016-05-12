using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public int worldSize = 10;
    public RoomGenerator generator;
    public RoomCreator creator;


    // Use this for initialization
    void Start () {
        generator.createMap();
        creator.performCreation(generator);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
