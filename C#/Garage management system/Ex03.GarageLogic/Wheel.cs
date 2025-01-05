using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName = "Michelin";
        private float m_CurrentAirPressure;
        private float m_MaximumAirPressureSetByTheManufacturer;

        public string ManufacturerName { get => m_ManufacturerName; set => m_ManufacturerName = value; }
        public float CurrentAirPressure { get => m_CurrentAirPressure; set => m_CurrentAirPressure = value; }
        public float MaximumAirPressureSetByTheManufacturer { get => m_MaximumAirPressureSetByTheManufacturer; set => m_MaximumAirPressureSetByTheManufacturer = value; }

        public void AirInflation(float i_NumberOfAirToAdd)
        {
            try
            {
                float newAirPressure = this.CurrentAirPressure + i_NumberOfAirToAdd;

                if (newAirPressure > m_MaximumAirPressureSetByTheManufacturer || i_NumberOfAirToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, m_MaximumAirPressureSetByTheManufacturer - m_CurrentAirPressure);
                }

                this.CurrentAirPressure = newAirPressure;
            }
            catch (ValueOutOfRangeException vore)
            {
                Console.WriteLine(vore.Message);
            }
        }
    }
}

