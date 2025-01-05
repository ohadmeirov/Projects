using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private static List<Vehicle> m_vehiclesInTheGarage = new List<Vehicle>();

        public static List<Vehicle> VehiclesInTheGarage
        {
            get { return m_vehiclesInTheGarage; }
            set { m_vehiclesInTheGarage = value; }
        }

        public static void AddVehicleToGarage(Vehicle vehicle)
        {
            m_vehiclesInTheGarage.Add(vehicle);
        }

        public static Vehicle GetVehicleByLicensePlate(string licensePlate)
        {
            return m_vehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);
        }
    }

}
