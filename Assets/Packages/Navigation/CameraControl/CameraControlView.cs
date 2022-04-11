using ShootCommon.Views.Mediation;
using UnityEngine;

namespace Packages.Navigation.CameraControl
{
    public class CameraControlView: View
    {
        public float speed = 0.1f;
        public void SetPosition(Vector2 position)
        {
            Vector3 mewposition = gameObject.transform.position;
            mewposition.x = position.x;
            mewposition.z = position.y;
            gameObject.transform.position = mewposition;
        }
    }
}