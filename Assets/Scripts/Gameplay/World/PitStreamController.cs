using System;
using System.Collections.Generic;
using IntoTheVoid.Gameplay.Speed;
using UnityEngine;

namespace IntoTheVoid.Gameplay.World
{
    public sealed class PitStreamController : MonoBehaviour
    {
        [SerializeField] private FallSpeedController fallSpeed;
        [SerializeField] private PitSlice[] slicePrefabs;
        [SerializeField, Min(2)] private int activeSliceCount = 6;
        [SerializeField] private float firstSliceCenterY;
        [SerializeField] private float recycleCenterY = 25f;
        [SerializeField, Min(0f)] private float speedMultiplier = 1f;
        [SerializeField] private int randomSeed;

        private readonly List<PitSlice> activeSlices = new();
        private System.Random random;

        public event Action<float> Scrolled;

        public float ScrolledDistance { get; private set; }

        private void Awake()
        {
            random = randomSeed == 0 ? new System.Random() : new System.Random(randomSeed);
            BuildInitialStream();
        }

        private void Update()
        {
            if (fallSpeed == null || activeSlices.Count == 0)
            {
                return;
            }

            Advance(fallSpeed.CurrentSpeed * speedMultiplier * Time.deltaTime);
        }

        public void Advance(float distance)
        {
            if (distance <= 0f)
            {
                return;
            }

            Vector3 movement = Vector3.up * distance;
            foreach (PitSlice slice in activeSlices)
            {
                slice.transform.Translate(movement, Space.World);
            }

            ScrolledDistance += distance;
            Scrolled?.Invoke(distance);
            RecyclePassedSlices();
        }

        private void BuildInitialStream()
        {
            if (slicePrefabs == null || slicePrefabs.Length == 0)
            {
                Debug.LogError($"{nameof(PitStreamController)} on '{name}' requires at least one slice prefab.", this);
                enabled = false;
                return;
            }

            float nextCenterY = firstSliceCenterY;
            PitSlice previous = null;

            for (int i = 0; i < activeSliceCount; i++)
            {
                PitSlice prefab = GetRandomPrefab();
                PitSlice slice = Instantiate(prefab, transform);

                if (previous != null)
                {
                    nextCenterY -= (previous.Length + slice.Length) * 0.5f;
                }

                slice.transform.localPosition = new Vector3(0f, nextCenterY, 0f);
                slice.PrepareForReuse();
                activeSlices.Add(slice);
                previous = slice;
            }
        }

        private void RecyclePassedSlices()
        {
            for (int i = 0; i < activeSlices.Count; i++)
            {
                PitSlice slice = activeSlices[i];
                if (slice.transform.position.y - slice.Length * 0.5f <= recycleCenterY)
                {
                    continue;
                }

                PitSlice lowest = FindLowestSlice(slice);
                float newCenterY = lowest.transform.position.y - (lowest.Length + slice.Length) * 0.5f;
                slice.transform.position = new Vector3(transform.position.x, newCenterY, transform.position.z);
                slice.PrepareForReuse();
            }
        }

        private PitSlice FindLowestSlice(PitSlice ignored)
        {
            PitSlice lowest = null;
            foreach (PitSlice slice in activeSlices)
            {
                if (slice == ignored)
                {
                    continue;
                }

                if (lowest == null || slice.transform.position.y < lowest.transform.position.y)
                {
                    lowest = slice;
                }
            }

            return lowest;
        }

        private PitSlice GetRandomPrefab()
        {
            return slicePrefabs[random.Next(slicePrefabs.Length)];
        }
    }
}
