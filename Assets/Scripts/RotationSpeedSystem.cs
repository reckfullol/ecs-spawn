using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSpeedSystem : JobComponentSystem {
    private ComponentGroup _componentGroup;

    protected override void OnCreateManager() {
        _componentGroup = GetComponentGroup(typeof(Rotation), ComponentType.ReadOnly<RotationSpeed>());
    }

    [BurstCompile]
    private struct RotationSpeedJob : IJobChunk {
        public float DeltaTime;
        public ArchetypeChunkComponentType<Rotation> RotationType;
        [ReadOnly] public ArchetypeChunkComponentType<RotationSpeed> RotationSpeedType;
        
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex) {
            var chunkRotations = chunk.GetNativeArray(RotationType);
            var chunkRotationSpeeds = chunk.GetNativeArray(RotationSpeedType);
            for (var i = 0; i < chunk.Count; i++) {
                var rotation = chunkRotations[i];
                var rotationSpeed = chunkRotationSpeeds[i];

                chunkRotations[i] = new Rotation {
                    Value = math.mul(math.normalize(rotation.Value),
                        quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * DeltaTime))
                };
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        var rotationType = GetArchetypeChunkComponentType<Rotation>();
        var rotationSpeedType = GetArchetypeChunkComponentType<RotationSpeed>();

        var job = new RotationSpeedJob {
            DeltaTime = Time.deltaTime,
            RotationType = rotationType,
            RotationSpeedType = rotationSpeedType
        };

        return job.Schedule(_componentGroup, inputDeps);
    }
}