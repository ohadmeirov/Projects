using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private eColorsOfCar m_ColorOfCar;
        private int m_NumberOfDoors;
        private eFuelType m_FuelTypeForFuelCar;
        private float m_MaximumFuelNumberInTheTankForFuelCar;
        private float m_MaximumBatteryTimeForElecticCar;
        private IPowerSupplier m_PowerSupplierType;

        public eColorsOfCar ColorOfCar { get => m_ColorOfCar; set => m_ColorOfCar = value; }
        public int NumberOfDoors { get => m_NumberOfDoors; set => m_NumberOfDoors = value; }
        public eFuelType FuelTypeForFuelCar { get => m_FuelTypeForFuelCar; set => m_FuelTypeForFuelCar = value; }
        public float MaximumFuelNumberInTheTankForFuelCar { get => m_MaximumFuelNumberInTheTankForFuelCar; set => m_MaximumFuelNumberInTheTankForFuelCar = value; }
        public float MaximumBatteryTimeForElecticCar { get => m_MaximumBatteryTimeForElecticCar; set => m_MaximumBatteryTimeForElecticCar = value; }


        public Car(IPowerSupplier i_PowerSupplierType, eColorsOfCar i_Color, int i_NumberOfDoors) : base(i_PowerSupplierType)
        {
            m_PowerSupplierType = i_PowerSupplierType;
            Wheels = new Wheel[5];
            m_ColorOfCar = i_Color;
            m_NumberOfDoors = i_NumberOfDoors;

            foreach(Wheel wheel in Wheels) 
            {
                wheel.MaximumAirPressureSetByTheManufacturer = 34;
            }

            if(m_PowerSupplierType is FuelTank)
            {
                m_FuelTypeForFuelCar = eFuelType.Octan95;
                m_MaximumFuelNumberInTheTankForFuelCar = 52f;
            }
            else if (m_PowerSupplierType is ElectricBattery)
            {
                m_MaximumBatteryTimeForElecticCar = 5.4f;
            }
            else {
                throw new ArgumentException();
            }
        }

        override public void Refuel(float i_NumberOfFuelToAdd, eFuelType i_FuelTypeToAdd)
        {
            if (m_PowerSupplierType is FuelTank)
            {
                m_PowerSupplierType.Refuel(this, i_NumberOfFuelToAdd, this.m_FuelTypeForFuelCar, i_FuelTypeToAdd, m_MaximumFuelNumberInTheTankForFuelCar);
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
                m_PowerSupplierType.Charge(this, i_NumberOfHoursToAdd, m_MaximumBatteryTimeForElecticCar);
            }
            else
            {
                throw new ArgumentException();
            }
        }                
    }
}
