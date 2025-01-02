using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectricBattery : IPowerSupplier
    {
        public void Charge(Vehicle vehicle, float i_NumberOfHoursToAdd, float i_maximumBatteryTimeForElecticCar)
        {
            if (vehicle.GetType() != typeof(ElectricBattery))
            {
                throw new ArgumentException();
            }

            try
            {
                float newTimeNumberToCharge = vehicle.PercentageOfEnergyRemaining + i_NumberOfHoursToAdd;

                if (newTimeNumberToCharge > i_maximumBatteryTimeForElecticCar || i_NumberOfHoursToAdd < 0)
                {
                    throw new ValueOutOfRangeException(0, i_maximumBatteryTimeForElecticCar - vehicle.PercentageOfEnergyRemaining);
                }

                vehicle.PercentageOfEnergyRemaining = newTimeNumberToCharge;
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
        public void Refuel(Vehicle i_Car, float i_NumberOfFuelToAdd, eFuelType i_VehicleFuelType, eFuelType i_FuelType, float i_MaximumFuelNumberInTheTankForFuelCar)
        {
            throw new ArgumentException();
        }
    }
}

    

