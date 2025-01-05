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

        public Motorcycle(IPowerSupplier i_PowerSupplierType, eLicenseTypeOfMotorcycle i_LicenseTypeOfMotorcycle, int i_EngineCapacity, float i_CurrentAirPressure) : base(i_PowerSupplierType)
        {
            m_PowerSupplierType = i_PowerSupplierType;
            Wheels = new Wheel[2];
            m_LicenseTypeOfMotorcycle = i_LicenseTypeOfMotorcycle;
            m_EngineCapacity = i_EngineCapacity;

            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i] = new Wheel();
            }

            foreach (Wheel wheel in Wheels)
            {
                wheel.MaximumAirPressureSetByTheManufacturer = 32;
                if (i_CurrentAirPressure < 0 || i_CurrentAirPressure > wheel.MaximumAirPressureSetByTheManufacturer)
                {
                    Console.WriteLine($"Invalid air pressure. Please enter a value between 0 and {wheel.MaximumAirPressureSetByTheManufacturer}.");

                    while (i_CurrentAirPressure < 0 || i_CurrentAirPressure > wheel.MaximumAirPressureSetByTheManufacturer)
                    {
                        Console.Write("Enter a valid air pressure: ");

                        if (float.TryParse(Console.ReadLine(), out i_CurrentAirPressure))
                        {
                            if (i_CurrentAirPressure < 0 || i_CurrentAirPressure > wheel.MaximumAirPressureSetByTheManufacturer)
                            {
                                Console.WriteLine($"Invalid input. Value must be between 0 and {wheel.MaximumAirPressureSetByTheManufacturer}.");
                            }
                        }
                        else
                        {
                            throw new FormatException("Invalid format provided.");
                        }
                    }
                }

                wheel.CurrentAirPressure = i_CurrentAirPressure;
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
