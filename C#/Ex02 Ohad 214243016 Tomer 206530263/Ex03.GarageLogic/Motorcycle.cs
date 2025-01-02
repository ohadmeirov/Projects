using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private int m_EngineCapacity;
        private eFuelType m_FuelTypeForFuelCar;
        private float m_MaximumFuelNumberInTheTankForFuelMotorcycle;
        private float m_MaximumBatteryTimeForElecticMotorcycle;
        private eLicenseTypeOfMotorcycle m_LicenseTypeOfMotorcycle;
        private IPowerSupplier m_PowerSupplierType;

        public int EngineCapacity { get => m_EngineCapacity; set => m_EngineCapacity = value; }
        public eFuelType FuelTypeForFuelCar { get => m_FuelTypeForFuelCar; set => m_FuelTypeForFuelCar = value; }
        public float MaximumFuelNumberInTheTankForFuelCar { get => m_MaximumFuelNumberInTheTankForFuelMotorcycle; set => m_MaximumFuelNumberInTheTankForFuelMotorcycle = value; }
        public float MaximumBatteryTimeForElecticCar { get => m_MaximumBatteryTimeForElecticMotorcycle; set => m_MaximumBatteryTimeForElecticMotorcycle = value; }
        public eLicenseTypeOfMotorcycle LicenseTypeOfMotorcycle { get => m_LicenseTypeOfMotorcycle; set => m_LicenseTypeOfMotorcycle = value; }

        public Motorcycle(IPowerSupplier i_PowerSupplierType, eLicenseTypeOfMotorcycle i_LicenseTypeOfMotorcycle, int i_EngineCapacity) : base(i_PowerSupplierType)
        {
            m_PowerSupplierType = i_PowerSupplierType;
            Wheels = new Wheel[2];
            m_LicenseTypeOfMotorcycle = i_LicenseTypeOfMotorcycle;
            m_EngineCapacity = i_EngineCapacity;

            foreach (Wheel wheel in Wheels)
            {
                wheel.MaximumAirPressureSetByTheManufacturer = 32;
            }

            if (i_PowerSupplierType is FuelTank)
            {
                m_FuelTypeForFuelCar = eFuelType.Octan98;
                m_MaximumFuelNumberInTheTankForFuelMotorcycle = 6.2f;
            }
            else if (i_PowerSupplierType is ElectricBattery)
            {
                m_MaximumBatteryTimeForElecticMotorcycle = 2.9f;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        override public void Refuel(float i_NumberOfFuelToAdd, eFuelType i_FuelTypeToAdd)
        {
            if (m_PowerSupplierType is FuelTank)
            {
                m_PowerSupplierType.Refuel(this, i_NumberOfFuelToAdd, this.m_FuelTypeForFuelCar, i_FuelTypeToAdd, m_MaximumFuelNumberInTheTankForFuelMotorcycle);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        override public void Charge(float i_NumberOfHoursToAdd)
        {
            if (m_PowerSupplierType is ElectricBattery)
            {
                m_PowerSupplierType.Charge(this, i_NumberOfHoursToAdd, m_MaximumBatteryTimeForElecticMotorcycle);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
