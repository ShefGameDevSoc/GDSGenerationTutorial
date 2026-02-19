using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Slider slider;
    
    private int[,] grid = new int[64, 64];
    
    public void RunRandomSpawn()
    {
        var tiles = RandomSpawn.Run(slider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    
    public void RunPerlinSpawn()
    {
        var tiles = PerlinSpawn.Run(-0.5f + slider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    public void RunWFCSpawn()
    {
        var tiles = BoundaryWFCSpawn.Run(slider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }
}
