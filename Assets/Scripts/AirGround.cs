using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AirGround : MonoBehaviour
{
    public static AirGround instance;

    public TilemapCollider2D tilemapCollider2D;
    public CompositeCollider2D compositeCollider2D;
    void Awake()
    {
        AirGround.instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        tilemapCollider2D = AirGround.instance.GetComponent<TilemapCollider2D>();
        compositeCollider2D = AirGround.instance.GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
