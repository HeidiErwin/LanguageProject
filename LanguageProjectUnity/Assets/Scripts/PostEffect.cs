using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour {
    Camera AttachedCamera;
    public Shader Post_Outline;

    void Start() {
        AttachedCamera = GetComponent<Camera>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
 
    }
}
