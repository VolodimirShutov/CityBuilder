using ShootCommon.Signals;
using UnityEngine;

namespace Packages.Navigation.Signals
{
    public class UpdateMousePositionSignal : Signal
    {
        public float Distance;
        public Ray Ray;
        public int TouchId;
        public Vector2 FingerPosition;
    }
}