using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBehavior : MonoBehaviour
{
    [SerializeField] GameObject arrowImage = null;

    public void SetPosition(Vector2 newPosition)
    {
        arrowImage.transform.position = new Vector3(newPosition.x, newPosition.y, arrowImage.transform.position.z);
    }

    public void SetRotation(GameObject target)
    {
        Vector2 direction = new Vector2(target.transform.position.x - arrowImage.transform.position.x, target.transform.position.y - arrowImage.transform.position.y);
        arrowImage.transform.right = direction;
        //arrowImage.transform.position = Vector3.right  * direction;
    }

}
