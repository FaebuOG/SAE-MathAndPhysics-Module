using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
   [SerializeField] private Renderer textureRender;
   [SerializeField] private MeshFilter meshFilter;
   [SerializeField] private MeshRenderer meshRenderer;

   public void DrawTexture(Texture2D texture2D)
   {
      textureRender.sharedMaterial.mainTexture = texture2D;
      textureRender.transform.localScale = new Vector3(texture2D.width, 1, texture2D.height);
   }

   public void DrawMesh(MeshData meshData, Texture2D texture2D)
   {
      meshFilter.sharedMesh = meshData.CreateMesh();
      meshRenderer.sharedMaterial.mainTexture = texture2D;
   }
}
