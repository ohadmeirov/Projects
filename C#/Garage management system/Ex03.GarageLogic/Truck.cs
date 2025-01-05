using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_IsDeliveredInColdTemperature;
        private eFuelType m_FuelTypeForFuelVehicle;
        private float m_MaximumFuelNumberInTheTankForFuelTruck;
        private float m_BagageCapacity;
        private IPowerSupplier m_PowerSupplierType;
        public bool IsDeliveredInColdTemperature { get => m_IsDeliveredInColdTemperature; set => m_IsDeliveredInColdTemperature = value; }
        public eFuelType FuelTypeForFuelCar { get => m_FuelTypeForFuelVehicle; set => m_FuelTypeForFuelVehicle = value; }
        public float MaximumFuelNumberInTheTankForFuelCar { get => m_MaximumFuelNumberInTheTankForFuelTruck; set => m_MaximumFuelNumberInTheTankForFuelTruck = value; }
        public float BagageCapacity { get => m_BagageCapacity; set => m_BagageCapacity = value; }

        public Truck(IPowerSupplier i_PowerSupplierType, float i_BagageCapacity, bool i_IsDeliveredInColdTemperature, float i_CurrentAirPressure) : base(i_PowerSupplierType)
        {
            m_PowerSupplierType = i_PowerSupplierType;
            Wheels = new Wheel[14];
            BagageCapacity = i_BagageCapacity;
            m_IsDeliveredInColdTemperature = i_IsDeliveredInColdTemperature;

            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i] = new Wheel();
            }

            foreach (Wheel wheel in Wheels)
            {
                wheel.MaximumAirPressureSetByTheManufacturer = 29;
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
          
            m_FuelTypeForFuelVehicle = eFuelType.Soler;
            m_MaximumFuelNumberInTheTankForFuelTruck = 125f;           
        }

        override public void Refuel(float i_NumberOfFuelToAdd, eFuelType i_FuelTypeToAdd)
        {
            m_PowerSupplierType.Refuel(this, i_NumberOfFuelToAdd, eFuelType.Soler, i_FuelTypeToAdd, m_MaximumFuelNumberInTheTankForFuelTruck);            
        }

        override public void Charge(float i_NumberOfHoursToAdd)
        {          
            throw new ArgumentException();           
        }
    }
}
