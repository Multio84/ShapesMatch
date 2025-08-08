using UnityEngine;


public class TutorialElement : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;

    void Start()
    {
        mat = GetComponent<Material>();

        //var pos = new Vector2(960, 540); // где нужна дырка
        mat.SetVector("_HoleCenter", pos);
        mat.SetFloat("_HoleRadius", 150);
    }
}
