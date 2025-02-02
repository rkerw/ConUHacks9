using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Vector3 axis;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis *speed * Time.deltaTime);
    }
}
