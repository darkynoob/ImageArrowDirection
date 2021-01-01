using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField] private GameObject imageCenter;
    private ImageBehavior imagenBehaviour;
    //private InputController inputController;

    // Start is called before the first frame update
    void Start()
    {
        imagenBehaviour = GetComponent<ImageBehavior>();
        //inputController = GetComponent<InputController>();
    }

    public void Awake()
    {
        InputController.OnSwipe += OnInput;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInput(SwipeData data)
    {
        MoveImage(data.EndPosition);
        imagenBehaviour.SetRotation(imageCenter);
    }

    private void MoveImage(Vector2 data)
    {
        Vector2 newPosition = new Vector2(data.x, data.y);
        imagenBehaviour.SetPosition(newPosition);
    }

}
