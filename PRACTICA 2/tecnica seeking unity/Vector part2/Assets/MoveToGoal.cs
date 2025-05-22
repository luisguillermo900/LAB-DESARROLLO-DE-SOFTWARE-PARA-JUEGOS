using UnityEngine;

public class MoveToGoal : MonoBehaviour {

    // Object speed
    public float speed = 2.0f;
    // Accuracy
    public float accuracy = 0.01f;
    // Object target
    public Transform goal;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void LateUpdate() {

        // Calculate the facing direction
        this.transform.LookAt(goal.position);

        // Find the the direction vector
        Vector3 direction = goal.position - this.transform.position;

        // Draw debug ray in the direction your object is facing
        Debug.DrawRay(this.transform.position, direction, Color.red);

        // Check how close you character is to the target
        if (direction.magnitude > accuracy) {

            // Move the Object
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
