using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_ModelName;
        private string m_LicensePlate;
        private float m_PercentageOfEnergyRemaining;
        private Wheel[] m_Wheels;
        private string m_OwnerName;
        private string m_OwnerPhoneNumber;
        private eConditionOfTheVehicleInTheGarage m_ConditionOfTheVehicleInTheGarage1 = eConditionOfTheVehicleInTheGarage.InRepair;
        private IPowerSupplier m_PowerSupplier;

        public string ModelName { get => m_ModelName; set => m_ModelName = value; }
        public string LicensePlate { get => m_LicensePlate; set => m_LicensePlate = value; }
        public float PercentageOfEnergyRemaining { get => m_PercentageOfEnergyRemaining; set => m_PercentageOfEnergyRemaining = value; }
        public string OwnerName { get => m_OwnerName; set => m_OwnerName = value; }
        public string OwnerPhoneNumber { get => m_OwnerPhoneNumber; set => m_OwnerPhoneNumber = value; }
        public eConditionOfTheVehicleInTheGarage ConditionOfTheVehicleInTheGarage1 { get => m_ConditionOfTheVehicleInTheGarage1; set => m_ConditionOfTheVehicleInTheGarage1 = value; }
        public Wheel[] Wheels { get => m_Wheels; set => m_Wheels = value; }
        public IPowerSupplier PowerSupplier { get => m_PowerSupplier; set => m_PowerSupplier = value; }

        public Vehicle(IPowerSupplier i_PowerSupplier)
        {
            PowerSupplier = i_PowerSupplier;
        }

        public abstract void Refuel(float i_NumberOfFuelToAdd, eFuelType i_FuelTypeToAdd); 
        public abstract void Charge(float i_NumberOfHoursToAdd);
    }
}
