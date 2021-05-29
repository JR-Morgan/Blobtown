using UnityEngine;

[AddComponentMenu("Simulation/Editor/Generate Button")]
[RequireComponent(typeof(TileGenerator))]
public class LevelGenerateButton : MonoBehaviour
{
    private TileGenerator _gen;
    private TileGenerator Gen { get
        {
            if (_gen == null)
            {
                _gen = GetComponent<TileGenerator>();
            }
            return _gen;
        }
    }

    public void Generate()
    {
        Gen.Generate();
    }

}
