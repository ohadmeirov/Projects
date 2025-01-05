using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public interface IPowerSupplier
    {
        void Charge(Vehicle vehicle, float i_NumberOfHoursToAdd, float i_maximumBatteryTimeForElecticCar);
        void Refuel(Vehicle vehicle, float i_NumberOfFuelToAdd, eFuelType i_VehicleFuelType, eFuelType i_FuelType, float i_MaximumFuelNumberInTheTankForFuelCar);
    }
}
