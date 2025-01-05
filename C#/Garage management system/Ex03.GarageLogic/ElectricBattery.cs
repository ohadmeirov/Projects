using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectricBattery : IPowerSupplier
    {
        public void Charge(Vehicle i_vehicle, float i_NumberOfHoursToAdd, float i_maximumBatteryTimeForElecticCar)
        {
            try
            {
                float newTimeNumberToCharge = i_vehicle.PercentageOfEnergyRemaining + i_NumberOfHoursToAdd;

                if (newTimeNumberToCharge > i_maximumBatteryTimeForElecticCar || i_NumberOfHoursToAdd < 0)
                {
                    float allowedChargeTime = i_maximumBatteryTimeForElecticCar - i_vehicle.PercentageOfEnergyRemaining;
                    throw new ValueOutOfRangeException(0, allowedChargeTime);
                }

                i_vehicle.PercentageOfEnergyRemaining = newTimeNumberToCharge;
                Console.WriteLine($"The vehicle has been charged. Current battery level: {i_vehicle.PercentageOfEnergyRemaining} hours.");
            }
            catch (ValueOutOfRangeException vore)
            {
                Console.WriteLine($"Error: {vore.Message}. You can add up to {vore.MaxValue} hours.");
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"Argument Error: {ae.Message}");
            }
        }

        public void Refuel(Vehicle i_vehicle, float i_NumberOfFuelToAdd, eFuelType i_VehicleFuelType, eFuelType i_FuelType, float i_MaximumFuelNumberInTheTankForFuelCar)
        {
            throw new ArgumentException();
        }
    }
}

    

