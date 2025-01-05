using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.ConsoleUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
        }


        public static void ShowMenu()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Hello User!\n" +
                                  "Please type the number of the requested service.\n" +
                                  "For adding new vehicle to the garage type 1.\n" +
                                  "To present the list of license plate which in the garage type 2.\n" +
                                  "To change the condition of a vehicle in the garage type 3.\n" +
                                  "To fill air in the wheels of a vehicle type 4.\n" +
                                  "To refuel a vehicle type 5.\n" +
                                  "To charge a vehicle type 6.\n" +
                                  "To present full details of a vehicle by a license plate type 7.\n" +
                                  "If you wanna exit, type 0.\n" +
                                  "Thank You Very Much And Have A Good Day! Waiting for your respond...");
                string userRespond = Console.ReadLine();

                switch (userRespond)
                {
                    case "0":
                        continueRunning = false;
                        break;
                    case "1":
                        GarageUISystem.AddNewVehicleToTheGarage();
                        break;
                    case "2":
                        GarageUISystem.ShowLicensePlatesWithOptionToFilteringByTheirCondition();
                        break;
                    case "3":
                        GarageUISystem.ChangeVehicleCondition();
                        break;
                    case "4":
                        GarageUISystem.FillAirInWheelsForTheMaximum();
                        break;
                    case "5":
                        GarageUISystem.RefuelVehicle();
                        break;
                    case "6":
                        GarageUISystem.ChargeVehicle();
                        break;
                    case "7":
                        GarageUISystem.ShowAllTheDetailsForAVehicle();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a valid option (0-7).");
                        continue;
                }

                if (continueRunning)
                {
                    Console.WriteLine("Press '10' to return to the main menu or '0' to exit.");
                    string userOption = Console.ReadLine();

                    if (userOption == "10")
                    {
                        Console.Clear();
                    }
                    else if (userOption == "0")
                    {
                        continueRunning = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please choose '10' to return to the main menu or '0' to exit.");
                    }
                }
            }

        }
    }
}
