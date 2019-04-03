using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class RotationSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public float DegreesPerSecond;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var data = new RotationSpeed {
            RadiansPerSecond = math.radians(DegreesPerSecond)
        };
        dstManager.AddComponentData(entity, data);
    }
}