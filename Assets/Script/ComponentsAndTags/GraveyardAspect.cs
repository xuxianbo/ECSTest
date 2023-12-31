﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.Zombie
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        // public readonly Entity entity;

        readonly RefRW<LocalTransform> _transform;
        readonly RefRO<GraveyardProperties> _roGraveyardProerties;
        readonly RefRW<GraveyardRandom> _rwGraveyardRandom;
        readonly RefRW<ZombieSpawnPoints> _rwZombieSpawnPoints;
        
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;
        
        // 数量
        public int NumberTombstonesToSpawn => _roGraveyardProerties.ValueRO.NumberTombstonesToSpawn;
        public Entity TombstonePrefab => _roGraveyardProerties.ValueRO.TobstonePrefab;
        
        public bool ZombieSpawnPointInitialized()
        {
            return _rwZombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
        }

        private ZombieSpawnPoints ZombieSpawnPoints => _rwZombieSpawnPoints.ValueRO;
        private int ZombieSpawnPointCount => _rwZombieSpawnPoints.ValueRO.Value.Value.Value.Length;



        private LocalTransform transform => _transform.ValueRO;
        private float3 MinCorner => transform.Position - HalfDimensions;
        private float3 MaxCorner => transform.Position + HalfDimensions;
        private const float BRAIN_SAFETY_RADIUS_SQ = 100;
        private float3 HalfDimensions => new()
        {
            x = _roGraveyardProerties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _roGraveyardProerties.ValueRO.FieldDimensions.y * 0.5f
        };

        private float3 GetRandomPosition()
        {
            float3 randomPos;
            do
            {
                randomPos = _rwGraveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(randomPos, transform.Position) <= BRAIN_SAFETY_RADIUS_SQ);

            return randomPos;
        }

        private quaternion GetRandomRotation() => quaternion.RotateY(_rwGraveyardRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
        private float getScale(float minScale) => _rwGraveyardRandom.ValueRW.Value.NextFloat(minScale, 1.0f);
        
        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = getScale(0.5f)
            };
        }

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.Value;
            set => _zombieSpawnTimer.ValueRW.Value = value;
        }
        public bool TimeToSpawnZombie => ZombieSpawnTimer <=0f;

        public float ZombieSpawnRate => _roGraveyardProerties.ValueRO.ZombieSpawnRate;

        public Entity ZombiePrefab => _roGraveyardProerties.ValueRO.ZombiePrefab;


        public LocalTransform GetZombieSpawnPoint()
        {
            var position = GetRandomZombieSpawnPoint();
            return new  LocalTransform
            {
                Position = position,
                Scale = 1F,
                Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, Position))
            };
        }

        private float3 GetRandomZombieSpawnPoint()
        {
            var index =_rwGraveyardRandom.ValueRW.Value.NextInt(ZombieSpawnPointCount);
            return _rwZombieSpawnPoints.ValueRW.Value.Value.Value[index];
        }

        public float3 Position => _transform.ValueRO.Position;
    }
}