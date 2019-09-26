using UnityEngine;

public class ConveyorBetlScript : MonoBehaviour
{
    [SerializeField] private float _beltSpeed = 20.1f;

    private Material mat;

    private void Awake()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.SetFloat("_ScrollYSpeed", _beltSpeed);
    }
}
