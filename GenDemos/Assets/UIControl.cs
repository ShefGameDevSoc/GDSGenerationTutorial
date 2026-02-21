using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Slider RandomSlider;
    public Slider PerlinSLider;
    public Slider WFCSlider;
    public GameObject RandomPannel;
    public GameObject PerlinPannel;
    public GameObject StepPannel;
    public GameObject WFCPannel;

    private void Start()
    {
        Clear();
    }

    private void Clear()
    {
        RandomPannel.SetActive(false);
        PerlinPannel.SetActive(false);
        StepPannel.SetActive(false);
        WFCPannel.SetActive(false);
    }

    private int[,] grid = new int[64, 64];
    
    public void RunRandomSpawn()
    {
        Clear();
        RandomPannel.SetActive(true);
        var tiles = RandomSpawn.Run(RandomSlider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    
    public void RunPerlinSpawn()
    {
        Clear();
        PerlinPannel.SetActive(true);
        var tiles = PerlinSpawn.Run(-0.5f + PerlinSLider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    public void RunWFCSpawn()
    {
        Clear();
        WFCPannel.SetActive(true);
        var tiles = BoundaryWFCSpawn.Setup(WFCSlider.value);
        SceneLoader.instance.RenderTiles(tiles);
    }

    private bool stepWFCPressed;
    public void StepWFCSpawnStart()
    {
        
        stepWFCPressed = true;
    }
    public void StepWFCSpawnSop()
    {
        
        stepWFCPressed = false;
    }

    void Update()
    {
        if (stepWFCPressed)
        {
            var tiles = BoundaryWFCSpawn.RunOnce();
            SceneLoader.instance.RenderTiles(tiles);    
        }
    }

    public void EndWFCSpawn()
    {
        var tiles = BoundaryWFCSpawn.RunToEnd();
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    public void RunWalkerSpawn()
    {
        Clear();
        StepPannel.SetActive(true);
        var tiles = WalkerSpawn.Setup();
        SceneLoader.instance.RenderTiles(tiles);
    }
    
    public void RunWalkerStep()
    {
        var tiles = WalkerSpawn.Walk();
        SceneLoader.instance.RenderTiles(tiles);
    }
}
