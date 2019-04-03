using System;
using Unity.Entities;

[Serializable]
public struct RotationSpeed : IComponentData {
    public float RadiansPerSecond;
}