using Ex03.GarageLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.ConsoleUI
{
    public static class GarageUISystem
    {
        public static void AddNewVehicleToTheGarage()
        {
            Console.WriteLine("Write the number of the license plate of the car:");
            string licensePlate = Console.ReadLine()?.Trim();
            bool isVehicleInTheGarage = false;

            foreach (var vehicle in Garage.VehiclesInTheGarage)
            {
                if (string.Equals(vehicle.LicensePlate?.Trim(), licensePlate, StringComparison.OrdinalIgnoreCase))
                {
                    isVehicleInTheGarage = true;
                    Console.WriteLine("This vehicle is already in the garage, now this vehicle is going to be in repair.");
                    vehicle.ConditionOfTheVehicleInTheGarage = eConditionOfTheVehicleInTheGarage.InRepair;
                    break;
                }
            }

            if (!isVehicleInTheGarage)
            {
                try
                {
                    string typeOfVehicle = GetVehicleType();
                    IPowerSupplier powerSupplierForTheVehicle = GetPowerSupplier();
                    float airPressure = GetAirPressure();

                    Vehicle newVehicle;

                    if (typeOfVehicle.Equals("Car", StringComparison.OrdinalIgnoreCase))
                    {
                        eColorsOfCar colorOfTheCar = GetCarColor();
                        int numberOfDoors = GetNumberOfDoors();
                        newVehicle = VehicleCreator.CreateVehicle(typeOfVehicle, powerSupplierForTheVehicle, airPressure, colorOfTheCar, numberOfDoors);
                    }
                    else if (typeOfVehicle.Equals("Motorcycle", StringComparison.OrdinalIgnoreCase))
                    {
                        eLicenseTypeOfMotorcycle licenseType = GetMotorcycleLicenseType();
                        int engineCapacity = GetEngineCapacity();
                        newVehicle = VehicleCreator.CreateVehicle(typeOfVehicle, powerSupplierForTheVehicle, airPressure, licenseType, engineCapacity);
                    }
                    else if (typeOfVehicle.Equals("Truck", StringComparison.OrdinalIgnoreCase))
                    {
                        bool isDeliveredInColdTemperature = GetIsDeliveredInColdTemperature();
                        float baggageCapacity = GetBagageCapacity();
                        newVehicle = VehicleCreator.CreateVehicle(typeOfVehicle, powerSupplierForTheVehicle, airPressure, isDeliveredInColdTemperature, baggageCapacity);
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    newVehicle.LicensePlate = licensePlate;
                    Garage.VehiclesInTheGarage.Add(newVehicle);
                    Console.WriteLine("Vehicle successfully added to the garage!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static string GetVehicleType()
        {
            string vehicleType;
            while (true)
            {
                Console.WriteLine("Please enter the vehicle type (Car, Motorcycle, Truck):");
                vehicleType = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(vehicleType) ||
                    !(vehicleType.Equals("Car", StringComparison.OrdinalIgnoreCase) ||
                      vehicleType.Equals("Motorcycle", StringComparison.OrdinalIgnoreCase) ||
                      vehicleType.Equals("Truck", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Vehicle type cannot be empty and must be one of: Car, Motorcycle, Truck. Please try again.");
                }
                else
                {
                    break;
                }
            }

            return vehicleType;
        }

        private static IPowerSupplier GetPowerSupplier()
        {
            string input;
            while (true)
            {
                Console.WriteLine("Please choose the power supplier (ElectricBattery, FuelTank):");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) ||
                    !(input.Equals("ElectricBattery", StringComparison.OrdinalIgnoreCase) ||
                      input.Equals("FuelTank", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Invalid power supplier type. Please choose either ElectricBattery or FuelTank. Please try again.");
                }
                else
                {
                    break;
                }
            }

            return VehicleCreator.CreatePowerSupplier(input);
        }



        public static eColorsOfCar GetCarColor()
        {
            while (true)
            {
                Console.WriteLine("Write what is the color of the car: ");
                string colorOfTheCarInput = Console.ReadLine();
                var colorDictionary = new Dictionary<string, eColorsOfCar>(StringComparer.OrdinalIgnoreCase)
        {
            { "blue", eColorsOfCar.Blue },
            { "black", eColorsOfCar.Black },
            { "grey", eColorsOfCar.Grey },
            { "white", eColorsOfCar.White }
        };

                if (colorDictionary.TryGetValue(colorOfTheCarInput, out eColorsOfCar colorOfTheCar))
                {
                    return colorOfTheCar;
                }
                else
                {
                    Console.WriteLine("Invalid color input. The color must be one of: Blue, Black, Grey, White. Please try again.");
                }
            }
        }

        public static float GetAirPressure()
        {
            while (true)
            {
                Console.WriteLine("Write how much air is left in the wheels: ");
                if (float.TryParse(Console.ReadLine(), out float airPressure) && airPressure >= 0)
                {
                    return airPressure;
                }
                else
                {
                    Console.WriteLine("Air pressure must be a non-negative number. Please try again.");
                }
            }
        }

        public static float GetBagageCapacity()
        {
            while (true)
            {
                Console.WriteLine("Write the baggage capacity of the truck: ");
                if (float.TryParse(Console.ReadLine(), out float baggageCapacity) && baggageCapacity >= 0)
                {
                    return baggageCapacity;
                }
                else
                {
                    Console.WriteLine("Baggage capacity must be a non-negative number. Please try again.");
                }
            }
        }

        public static int GetNumberOfDoors()
        {
            while (true)
            {
                Console.WriteLine("Write how many doors there are in the car: ");
                if (int.TryParse(Console.ReadLine(), out int numberOfDoors) && numberOfDoors > 0)
                {
                    return numberOfDoors;
                }
                else
                {
                    Console.WriteLine("Number of doors must be a positive integer. Please try again.");
                }
            }
        }

        public static int GetEngineCapacity()
        {
            while (true)
            {
                Console.WriteLine("Write the engine capacity of the motorcycle: ");
                if (int.TryParse(Console.ReadLine(), out int engineCapacity) && engineCapacity > 0)
                {
                    return engineCapacity;
                }
                else
                {
                    Console.WriteLine("Engine capacity must be a positive integer. Please try again.");
                }
            }
        }

        public static eLicenseTypeOfMotorcycle GetMotorcycleLicenseType()
        {
            while (true)
            {
                Console.WriteLine("Write the type of license is this motorcycle using: ");
                string typeOfLicenseInput = Console.ReadLine();

                switch (typeOfLicenseInput)
                {
                    case "A1":
                        return eLicenseTypeOfMotorcycle.A1;
                    case "A2":
                        return eLicenseTypeOfMotorcycle.A2;
                    case "B1":
                        return eLicenseTypeOfMotorcycle.B1;
                    case "B2":
                        return eLicenseTypeOfMotorcycle.B2;
                    default:
                        Console.WriteLine("Invalid license type. The license must be A1, A2, B1, or B2. Please try again.");
                        break;
                }
            }
        }

        public static bool GetIsDeliveredInColdTemperature()
        {
            while (true)
            {
                Console.WriteLine("Write if the truck drives in cold temperature (true/false): ");
                if (bool.TryParse(Console.ReadLine(), out bool isDeliveredInColdTemperature))
                {
                    return isDeliveredInColdTemperature;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'true' or 'false'. Please try again.");
                }
            }
        }

        public static void ShowLicensePlatesWithOptionToFilteringByTheirCondition()
        {
            List<Vehicle> vehiclesInTheGarage = Garage.VehiclesInTheGarage;
            Console.WriteLine("Would you like to filter the license plates by condition? (yes/no)");
            string filterChoice = Console.ReadLine()?.ToLower();

            while (filterChoice != "yes" && filterChoice != "no")
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                filterChoice = Console.ReadLine()?.ToLower();
            }

            List<Vehicle> filteredVehicles = new List<Vehicle>(vehiclesInTheGarage);

            if (filterChoice == "yes")
            {
                Console.WriteLine("Please choose the condition for filtering (InRepair, Repaired, Paid):");
                string conditionInput = Console.ReadLine()?.ToLower();
                List<string> validConditions = new List<string> { "inrepair", "repaired", "paid" };

                while (!validConditions.Contains(conditionInput))
                {
                    Console.WriteLine("Invalid input. Please enter one of the following: InRepair, Repaired, Paid.");
                    conditionInput = Console.ReadLine()?.ToLower();
                }

                eConditionOfTheVehicleInTheGarage condition;
                if (Enum.TryParse(conditionInput, true, out condition))
                {
                    filteredVehicles = vehiclesInTheGarage.Where(v => v.ConditionOfTheVehicleInTheGarage == condition).ToList();
                }
                else
                {
                    throw new ArgumentException("Invalid condition input. Valid options are InRepair, Repaired, Paid.");
                }
            }

            Console.WriteLine("Displaying license plates:");

            foreach (var vehicle in filteredVehicles)
            {
                Console.WriteLine(vehicle.LicensePlate);
            }
        }

        public static void ChangeVehicleCondition()
        {
            Console.WriteLine("Please enter the license plate of the vehicle you want to change the condition for:");
            string licensePlate = Console.ReadLine();
            Vehicle vehicleToChangeCondition = Garage.VehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);

            if (vehicleToChangeCondition == null)
            {
                throw new ArgumentException("Vehicle with the specified license plate not found.");
            }

            Console.WriteLine("Please enter the new condition for the vehicle (InRepair, Repaired, Paid):");
            string newConditionInput = Console.ReadLine()?.ToLower();
            List<string> validConditions = new List<string> { "inrepair", "repaired", "paid" };

            while (!validConditions.Contains(newConditionInput))
            {
                Console.WriteLine("Invalid condition input. Please enter one of the following: InRepair, Repaired, Paid.");
                newConditionInput = Console.ReadLine()?.ToLower();
            }

            eConditionOfTheVehicleInTheGarage newCondition;
            if (Enum.TryParse(newConditionInput, true, out newCondition))
            {
                vehicleToChangeCondition.ConditionOfTheVehicleInTheGarage = newCondition;
                Console.WriteLine($"The condition of the vehicle with license plate {licensePlate} has been updated to {newCondition}.");
            }
            else
            {
                throw new ArgumentException("Invalid condition input. Valid options are InRepair, Repaired, Paid.");
            }
        }

        public static void FillAirInWheelsForTheMaximum()
        {
            Console.WriteLine("Please enter the license plate of the vehicle you want to fill the air of the wheels for the maximum:");
            string licensePlate = Console.ReadLine();
            Vehicle vehicleToChangeAirOfWheel = Garage.VehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);

            if (vehicleToChangeAirOfWheel == null)
            {
                throw new ArgumentException("Vehicle with the specified license plate not found.");
            }

            foreach (var wheel in vehicleToChangeAirOfWheel.Wheels)
            {
                wheel.CurrentAirPressure = wheel.MaximumAirPressureSetByTheManufacturer;
            }
            Console.WriteLine("Air Succesfully Added!");
        }

        public static void RefuelVehicle()
        {
            Console.WriteLine("Please enter the license plate of the vehicle you want to refuel:");
            string licensePlate = Console.ReadLine();
            Vehicle vehicleToRefuel = Garage.VehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);

            if (vehicleToRefuel == null)
            {
                throw new ArgumentException("Vehicle with the specified license plate not found.");
            }

            Console.WriteLine("Write how much fuel you wanna add: ");
            float numberOfFuelToAdd;

            try
            {
                numberOfFuelToAdd = float.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid input. Please enter a valid number.", ex);
            }

            Console.WriteLine("Write which kind of fuel you wanna add (Octan95, Octan98, Soler): ");
            string fuelTypeInput = Console.ReadLine();
            Console.WriteLine("Vehicle Succesfully Refuled!");

            eFuelType fuelType;
            if (!Enum.TryParse(fuelTypeInput, true, out fuelType) || !Enum.IsDefined(typeof(eFuelType), fuelType))
            {
                throw new ArgumentException("Invalid fuel type. Please choose from Octan95, Octan98, Soler.");
            }

            if (vehicleToRefuel is Car && fuelType != eFuelType.Octan95)
            {
                throw new ArgumentException("Cars must use Octan95 fuel.");
            }
            if (vehicleToRefuel is Truck && fuelType != eFuelType.Soler)
            {
                throw new ArgumentException("Trucks must use Soler fuel.");
            }
            if (vehicleToRefuel is Motorcycle && fuelType != eFuelType.Octan98)
            {
                throw new ArgumentException("Motorcycles must use Octan98 fuel.");
            }

            vehicleToRefuel.Refuel(numberOfFuelToAdd, fuelType);
            
        }

        public static void ChargeVehicle()
        {
            Console.WriteLine("Please enter the license plate of the vehicle you want to charge:");
            string licensePlate = Console.ReadLine();
            Vehicle vehicleToRefuel = Garage.VehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);

            if (vehicleToRefuel == null)
            {
                throw new ArgumentException("Vehicle with the specified license plate not found.");
            }

            Console.WriteLine("Write how much energy you wanna add: ");
            float numberOfHoursToAdd;

            try
            {
                numberOfHoursToAdd = float.Parse(Console.ReadLine());
                Console.WriteLine("Vehicle Succesfully Charged!");
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid input. Please enter a valid number.", ex);
            }

            vehicleToRefuel.Charge(numberOfHoursToAdd);
            
        }

        public static void ShowAllTheDetailsForAVehicle()
        {
            Console.WriteLine("Please enter the license plate of the vehicle you want to look its details:");
            string licensePlate = Console.ReadLine();
            Vehicle vehicleToPresent = Garage.VehiclesInTheGarage.FirstOrDefault(v => v.LicensePlate == licensePlate);

            if (vehicleToPresent == null)
            {
                throw new ArgumentException("Vehicle with the specified license plate not found.");
            }

            Console.WriteLine(String.Format("License plate: {0},\n" +
                "Type name: {1},\n" +
                "Owner name: {2},\n" +
                "Condition in the garage: {3},\n" +
                "Wheels details: \n" +
                "Current air pressure: {4},\n" +
                "Manufacturer name: {5}\n", 
                vehicleToPresent.LicensePlate,
                vehicleToPresent.ModelName,
                vehicleToPresent.OwnerName,
                vehicleToPresent.ConditionOfTheVehicleInTheGarage,
                vehicleToPresent.Wheels[0].CurrentAirPressure,
                vehicleToPresent.Wheels[0].ManufacturerName));

            if (vehicleToPresent.TypeOfPowerSupplier == eTypeOfPowerSupplier.Fuel)
            {
                Console.WriteLine(String.Format("Amount of fuel left: {0},\n" +
                    "Type of fuel it uses: {1}\n",
                    vehicleToPresent.PercentageOfEnergyRemaining,
                    eFuelType.Octan95));
            }
            else if (vehicleToPresent.TypeOfPowerSupplier == eTypeOfPowerSupplier.ElectricBattery)
            {
                Console.WriteLine(String.Format("Amount of battery left: {0}\n",
                    vehicleToPresent.PercentageOfEnergyRemaining));
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}


