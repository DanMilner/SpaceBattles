using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private float m_CurrentFps;
        public UIHandler uIHandler;
        
        void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }

        void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = m_FpsAccumulator/fpsMeasurePeriod;
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                uIHandler.SetFPS(m_CurrentFps);
            }
        }
    }
}
