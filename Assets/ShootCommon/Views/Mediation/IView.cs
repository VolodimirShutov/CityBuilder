using UnityEngine;

namespace ShootCommon.Views.Mediation
{
    public interface IView
    {
        GameObject GetGameObject { get; }
    }
}