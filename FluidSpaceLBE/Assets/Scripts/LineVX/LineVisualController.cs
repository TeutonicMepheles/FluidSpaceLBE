using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualController : MonoBehaviour
{
    public Material lineVisualMaterial;
    public float[] lineDot;
    public float[] speed;
    public float[] dashes;
    
    private void OnEnable()
    {
        // 订阅传送功能开启的事件
        TeleportationActivator.TeleportEnalbed += OnLineShaderValid;
        TeleportationActivator.TeleportDisalbed += OnLineShaderUnvalid;
    }

    private void OnDisable()
    {
        // 取消订阅传送功能关闭的事件
        TeleportationActivator.TeleportEnalbed -= OnLineShaderValid;
        TeleportationActivator.TeleportDisalbed -= OnLineShaderUnvalid;
    }
    
    void OnLineShaderValid()
    {
        lineVisualMaterial.SetFloat("_NumOfDashes",dashes[0]);
        lineVisualMaterial.SetFloat("_NotValidLine",lineDot[0]);
        lineVisualMaterial.SetFloat("_Speed",speed[0]);
    }    
    void OnLineShaderUnvalid()
    {
        lineVisualMaterial.SetFloat("_NumOfDashes",dashes[1]);
        lineVisualMaterial.SetFloat("_NotValidLine",lineDot[1]);
        lineVisualMaterial.SetFloat("_Speed",speed[1]);
    }  

}
