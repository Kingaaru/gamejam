using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ProceduralLava : MonoBehaviour
{
    [Header("Lava Settings")]
    public int textureSize = 128;
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0.05f;
    
    [Tooltip("Increase this to make the lava look further away and more detailed!")]
    public Vector2 tiling = new Vector2(20f, 20f); 
    
    [Header("Colors")]
    public Color lavaBright = new Color(1f, 0.4f, 0.05f); 
    public Color lavaDark = new Color(0.5f, 0.05f, 0.0f);   

    private Texture2D lavaTexture;
    private Material lavaMaterial;
    private Vector2 uvOffset = Vector2.zero;

    void Start()
    {
        // Generate texture
        lavaTexture = new Texture2D(textureSize, textureSize);
        lavaTexture.filterMode = FilterMode.Bilinear;
        
        // CRITICAL FIX: Tell the texture to repeat instead of stretching!
        lavaTexture.wrapMode = TextureWrapMode.Repeat; 
        
        GenerateLavaPattern();

        Renderer rend = GetComponent<Renderer>();
        Shader unlitShader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Texture");
        if (unlitShader == null) unlitShader = Shader.Find("Standard");

        lavaMaterial = new Material(unlitShader);
        lavaMaterial.mainTexture = lavaTexture;
        
        // CRITICAL FIX: Apply the tiling scale
        lavaMaterial.mainTextureScale = tiling; 
        
        rend.material = lavaMaterial;
    }

    void Update()
    {
        // Unscaled time allows the lava to keep flowing even when Time.timeScale is 0!
        uvOffset.x += scrollSpeedX * Time.unscaledDeltaTime;
        uvOffset.y += scrollSpeedY * Time.unscaledDeltaTime;
        
        if (lavaMaterial != null)
        {
            lavaMaterial.mainTextureOffset = uvOffset;
        }
    }


    void GenerateLavaPattern()
    {
        Color[] pixels = new Color[textureSize * textureSize];
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float nx = (float)x / textureSize * 12f;
                float ny = (float)y / textureSize * 12f;

                float noise = Mathf.Sin(nx + Mathf.Sin(ny)) * Mathf.Cos(ny + Mathf.Cos(nx));
                float t = Mathf.InverseLerp(-1f, 1f, noise);

                pixels[y * textureSize + x] = Color.Lerp(lavaDark, lavaBright, t);
            }
        }
        lavaTexture.SetPixels(pixels);
        lavaTexture.Apply();
    }
}