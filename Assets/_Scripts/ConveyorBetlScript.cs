using UnityEngine;

public class ConveyorBetlScript : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float _beltSpeed = 20.1f;
    
    private void Awake()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.SetFloat("_ScrollYSpeed", _beltSpeed);
    }

}
