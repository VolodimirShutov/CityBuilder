using ShootCommon.Signals;
using UnityEngine;

namespace Packages.Navigation.Signals
{
    public class ClickMouseSignal : Signal
    {
        public float Distance;
        public Ray Ray;
        public int TouchId;
    }
}