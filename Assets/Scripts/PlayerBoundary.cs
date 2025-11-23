using UnityEngine;
public class PlayerBoundary : MonoBehaviour
{
    public Transform playerRoot;

    private BoxCollider box;

    private void Start() {
        box = GetComponent<BoxCollider>();
    }
    private void Update() {

    {
        //Gets the dimensions of box collider
        var bounds = box.bounds;

        Vector3 pos = playerRoot.position;

        //Clamp returns value of players x and z depending on player location relative to bounds
        //Ex: if player x is between the min and max x values return player position, if below minimum return minimum, if above max return max
        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.z = Mathf.Clamp(pos.z, bounds.min.z, bounds.max.z);
        
        playerRoot.position = pos;
    }
}
}