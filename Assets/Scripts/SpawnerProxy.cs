using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class SpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity {

    public int CountX;
    public int CountY;
    public GameObject Prefab;
    
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs) {
        referencedPrefabs.Add(Prefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        var spawnerData = new Spawner() {
            Prefab = conversionSystem.GetPrimaryEntity(this.Prefab),
            CountX = this.CountX,
            CountY = this.CountY
        };
        dstManager.AddComponentData(entity, spawnerData);

    }
}