using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawnerSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

    protected override void OnCreateManager() {
        _endSimulationEntityCommandBufferSystem = World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();
    }

    private struct SpawnJob : IJobProcessComponentDataWithEntity<Spawner, LocalToWorld> {
        public EntityCommandBuffer EntityCommandBuffer;
        
        
        public void Execute(Entity entity, int index, [ReadOnly] ref Spawner spawner, [ReadOnly] ref LocalToWorld localToWorld) {
            for (var i = 0; i < spawner.CountX; i++) {
                for (var j = 0; j < spawner.CountY; j++) {
                    var instance = EntityCommandBuffer.Instantiate(spawner.Prefab);
                    var position = math.transform(localToWorld.Value,
                        new float3(i * 1.3F, noise.cnoise(new float2(i, j) * 0.21F) * 2, j * 1.3F));
                    
                    EntityCommandBuffer.SetComponent(instance, new Translation() {
                        Value = position
                    });
                }
            }
            
            EntityCommandBuffer.DestroyEntity(entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        var job = new SpawnJob() {
            EntityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer()
        }.ScheduleSingle(this, inputDeps);
        
        _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}