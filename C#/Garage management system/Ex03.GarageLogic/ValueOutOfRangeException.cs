using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MaxValue;
        private float m_MinValue;

        public float MaxValue { get => m_MaxValue; set => m_MaxValue = value; }
        public float MinValue { get => m_MinValue; set => m_MinValue = value; }

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base($"Value is out of range! It should be between {i_MinValue} and {i_MaxValue}.")
        {
            m_MinValue = i_MinValue;
            m_MaxValue = i_MaxValue;
        }
    }

}
