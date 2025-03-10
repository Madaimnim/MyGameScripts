using System.Collections.Generic;
using UnityEngine;


public class CameraInstance : MonoBehaviour
{
    private static Dictionary<string, CameraInstance> instances = new Dictionary<string, CameraInstance>();

    private void Awake() {
        string cameraName = gameObject.name;

        // �p�G�������w�g���ۦP�W�٪��۾��A�R���ۤv
        if (instances.ContainsKey(cameraName))
        {
            Destroy(gameObject);
            return;
        }

        // �O���۾��A�T�O�ߤ@
        instances[cameraName] = this;
        DontDestroyOnLoad(gameObject); // ���۾��b���������ɫO��
    }
}
