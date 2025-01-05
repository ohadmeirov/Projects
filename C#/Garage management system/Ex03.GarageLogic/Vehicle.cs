using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName = "BMW";
        private string m_LicensePlate;
        private float m_PercentageOfEnergyRemaining = 1f;
        private Wheel[] m_Wheels;
        private string m_OwnerName = "Ohad";
        private string m_OwnerPhoneNumber = "111";
        private eConditionOfTheVehicleInTheGarage m_ConditionOfTheVehicleInTheGarage = eConditionOfTheVehicleInTheGarage.InRepair;
        private IPowerSupplier m_PowerSupplier;
        private eTypeOfPowerSupplier m_TypeOfPowerSupplier;

        public string ModelName { get => m_ModelName; set => m_ModelName = value; }
        public string LicensePlate { get => m_LicensePlate; set => m_LicensePlate = value; }
        public float PercentageOfEnergyRemaining { get => m_PercentageOfEnergyRemaining; set => m_PercentageOfEnergyRemaining = value; }
        public string OwnerName { get => m_OwnerName; set => m_OwnerName = value; }
        public string OwnerPhoneNumber { get => m_OwnerPhoneNumber; set => m_OwnerPhoneNumber = value; }
        public eConditionOfTheVehicleInTheGarage ConditionOfTheVehicleInTheGarage { get => m_ConditionOfTheVehicleInTheGarage; set => m_ConditionOfTheVehicleInTheGarage = value; }
        public Wheel[] Wheels { get => m_Wheels; set => m_Wheels = value; }
        public eTypeOfPowerSupplier TypeOfPowerSupplier { get => m_TypeOfPowerSupplier; set => m_TypeOfPowerSupplier = value; }

        public Vehicle(IPowerSupplier i_PowerSupplier)
        {
            m_PowerSupplier = i_PowerSupplier;
        }

        public abstract void Refuel(float i_NumberOfFuelToAdd, eFuelType i_FuelTypeToAdd); 
        public abstract void Charge(float i_NumberOfHoursToAdd);
    }
}
