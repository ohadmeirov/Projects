using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class VehicleCreator
    {
        private static readonly List<string> SupportedVehicleTypes = new List<string> { "Car", "Motorcycle", "Truck" };

        public static Vehicle CreateVehicle(string i_vehicleType, IPowerSupplier i_powerSupplier, float i_airPressure, params object[] i_additionalParameters)
        {
            if (!SupportedVehicleTypes.Contains(i_vehicleType, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Unsupported vehicle type. Supported types are: {string.Join(", ", SupportedVehicleTypes)}");
            }

            switch (i_vehicleType.ToLower())
            {
                case "car":
                    eColorsOfCar carColor = (eColorsOfCar)i_additionalParameters[0];
                    int numberOfDoors = (int)i_additionalParameters[1];
                    return new Car(i_powerSupplier, carColor, numberOfDoors, i_airPressure);

                case "motorcycle":
                    eLicenseTypeOfMotorcycle licenseType = (eLicenseTypeOfMotorcycle)i_additionalParameters[0];
                    int engineCapacity = (int)i_additionalParameters[1];
                    return new Motorcycle(i_powerSupplier, licenseType, engineCapacity, i_airPressure);

                case "truck":
                    bool isDeliveredInColdTemperature = (bool)i_additionalParameters[0];
                    float baggageCapacity = (float)i_additionalParameters[1];
                    return new Truck(i_powerSupplier, baggageCapacity, isDeliveredInColdTemperature, i_airPressure);

                default:
                    throw new ArgumentException($"Invalid vehicle type: {i_vehicleType}");
            }
        }

        public static IPowerSupplier CreatePowerSupplier(string i_powerSupplierType)
        {
            if (string.Equals(i_powerSupplierType, "ElectricBattery", StringComparison.OrdinalIgnoreCase))
            {
                return new ElectricBattery();
            }
            else if (string.Equals(i_powerSupplierType, "FuelTank", StringComparison.OrdinalIgnoreCase))
            {
                return new FuelTank();
            }
            else
            {
                throw new ArgumentException("Invalid power supplier type.");
            }
        }
    }

}
