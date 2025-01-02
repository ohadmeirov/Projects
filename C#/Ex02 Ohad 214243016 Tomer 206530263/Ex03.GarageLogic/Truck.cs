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

        public Truck(IPowerSupplier i_PowerSupplierType, float i_BagageCapacity, bool i_IsDeliveredInColdTemperature) : base(i_PowerSupplierType)
        {
            m_PowerSupplierType = i_PowerSupplierType;
            Wheels = new Wheel[14];
            BagageCapacity = i_BagageCapacity;
            m_IsDeliveredInColdTemperature = i_IsDeliveredInColdTemperature;

            foreach (Wheel wheel in Wheels)
            {
                wheel.MaximumAirPressureSetByTheManufacturer = 29;
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
