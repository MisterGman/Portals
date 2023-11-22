using UnityEngine;

namespace _Scripts.Utils
{
    public class Utils : MonoBehaviour
    {
        public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = 1;
            return camera.ScreenToWorldPoint(position);
        }
    }
}
