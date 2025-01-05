using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class FuelTank : IPowerSupplier
    {
        public void Refuel(Vehicle i_vehicle, float i_NumberOfFuelToAdd, eFuelType i_VehicleFuelType, eFuelType i_FuelType, float i_MaximumFuelNumberInTheTankForFuelCar)
        {                   
            try
            {
                float newFuelNumber = i_vehicle.PercentageOfEnergyRemaining  + i_NumberOfFuelToAdd;

                if (i_VehicleFuelType != i_FuelType)
                {
                    throw new ArgumentException();
                }
                else if (newFuelNumber > i_MaximumFuelNumberInTheTankForFuelCar || i_NumberOfFuelToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, i_MaximumFuelNumberInTheTankForFuelCar - i_vehicle.PercentageOfEnergyRemaining);
                }

                i_vehicle.PercentageOfEnergyRemaining = newFuelNumber;
                Console.WriteLine($"The vehicle has been refueld. Current fuel tank is: {i_vehicle.PercentageOfEnergyRemaining}.");
            }
            catch (ValueOutOfRangeException vore)
            {
                Console.WriteLine(vore.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
        public void Charge(Vehicle i_vehicle, float i_NumberOfHoursToAdd, float i_maximumBatteryTimeForElecticCar)
        {
            throw new ArgumentException();
        }
    }
}
