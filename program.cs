// Purpose: Assignment 4 - Creatting a simple console applicationthat will allow baldur gate merchant
// to manage and display the inventory and weapon details
//Author: Manraj Singh Wazir
// Date: 2025-04-22

// PS. i wanted to do something creative with the code but i had back to back workload and i was not able to do it. 
// dont be dissapointed :(

namespace Assignment4ManrajSinghWazir
{
    internal class Program
    {
        // Constant for the inventory file name - using snake case as per requirements
        const string INVENTORY_FILE = "inventory.csv";

        static void Main(string[] args)
        {
            // Display welcome ASCII art
            DisplayWelcomeArt();

            // Initialize a list to store weapon objects
            List<Weapon> weapons = new List<Weapon>();

            // Check if the inventory file exists and load weapons if it does
            if (File.Exists(INVENTORY_FILE))
            {
                LoadWeapons(weapons);
            }

            // Main program loop flag
            bool exit = false;

            // Main application loop - continues until user chooses to exit
            while (!exit)
            {
                // Display menu options and get user choice
                DisplayMainMenu();
                string choice = Console.ReadLine().ToUpper();

                // Process user choice with switch statement
                switch (choice)
                {
                    case "D": // Display all weapons in inventory
                        DisplayAllWeapons(weapons);
                        break;
                    case "S": // Search for a specific weapon
                        SearchWeapon(weapons);
                        break;
                    case "A": // Add a new weapon to inventory
                        Weapon newWeapon = AddWeapon(weapons);
                        weapons.Add(newWeapon);
                        GreenText("Weapon added successfully!");
                        break;
                    case "E": // Edit an existing weapon
                        EditWeapon(weapons);
                        break;
                    case "R": // Remove a weapon from inventory
                        RemoveWeapon(weapons);
                        break;
                    case "V": // View average cost of all weapons
                        ViewAverageCost(weapons);
                        break;
                    case "L": // List weapons by rarity
                        DisplayAllWeaponsByRarity(weapons);
                        break;
                    case "Q": // Save changes and exit the program
                        SaveWeapons(weapons);
                        DisplayGoodbyeArt();
                        exit = true;
                        break;
                    default: // Handle invalid menu choices
                        RedText("Invalid option. Try again.");
                        break;
                }
            }
        }

        // NEW ASCII ART METHODS
        static void DisplayWelcomeArt()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"▄▄▄▄·  ▄▄▄· ▄▄▌  ·▄▄▄▄  ▄• ▄▌▄▄▄  .▄▄ ·      ▄▄ •  ▄▄▄· ▄▄▄▄▄▄▄▄ .
▐█ ▀█▪▐█ ▀█ ██•  ██▪ ██ █▪██▌▀▄ █·▐█ ▀.     ▐█ ▀ ▪▐█ ▀█ •██  ▀▄.▀·
▐█▀▀█▄▄█▀▀█ ██▪  ▐█· ▐█▌█▌▐█▌▐▀▀▄ ▄▀▀▀█▄    ▄█ ▀█▄▄█▀▀█  ▐█.▪▐▀▀▪▄
██▄▪▐█▐█ ▪▐▌▐█▌▐▌██. ██ ▐█▄█▌▐█•█▌▐█▄▪▐█    ▐█▄▪▐█▐█ ▪▐▌ ▐█▌·▐█▄▄▌
·▀▀▀▀  ▀  ▀ .▀▀▀ ▀▀▀▀▀•  ▀▀▀ .▀  ▀ ▀▀▀▀     ·▀▀▀▀  ▀  ▀  ▀▀▀  ▀▀▀ ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== BALDUR'S GATE ===");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\"Best weapons this side of the Sword Coast!\"\n");
            Console.ResetColor();
        }

        static void DisplayGoodbyeArt()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"
   _____                 _ _                _ 
  / ____|               | | |              | |
 | |  __  ___   ___   __| | |__  _   _  ___| |
 | | |_ |/ _ \ / _ \ / _` | '_ \| | | |/ _ \ |
 | |__| | (_) | (_) | (_| | |_) | |_| |  __/_|
  \_____|\___/ \___/ \__,_|_.__/ \__, |\___(_)
                                  __/ |       
                                 |___/        
");
            Console.ResetColor();
            GreenText("Weapons saved. Goodbye!");
        }

        // HELPER METHODS
        // Save weapons to CSV file with exception handling for file operations
        static void SaveWeapons(List<Weapon> weapons)
        {
            try
            {
                // Use 'using' statement to automatically dispose of StreamWriter
                using (StreamWriter writer = new StreamWriter(INVENTORY_FILE))
                {
                    foreach (Weapon w in weapons)
                    {
                        writer.WriteLine(w.ToCSVString()); // Write each weapon on a new line
                    }
                }
                GreenText($"Weapons saved to {INVENTORY_FILE}.");
            }
            catch (IOException ex)
            {
                // Handle file access or write errors
                RedText($"Error writing to file: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Generic error handling for unexpected exceptions
                RedText($"Unexpected error saving weapons: {ex.Message}");
            }
        }

        // Display all weapons in inventory with detailed information
        // MODIFIED DISPLAY METHODS WITH COLOR CODING FOR RARITIES
        static void DisplayAllWeapons(List<Weapon> weapons)
        {
            if (weapons.Count == 0)
            {
                Console.WriteLine("No weapons in the inventory.");
                return;
            }

            Console.WriteLine("\n=== All Weapons in Inventory ===");
            Console.WriteLine("".PadRight(60, '='));

            foreach (Weapon w in weapons)
            {
                Console.ForegroundColor = GetRarityColor(w.Quality);
                Console.WriteLine(w.WeaponDetails());
                Console.ResetColor();
                Console.WriteLine("".PadRight(60, '-'));
            }
        }

        // Display all weapons ordered by rarity (shows less detailed information)
        static void DisplayAllWeaponsByRarity(List<Weapon> weapons)
        {
            if (weapons.Count == 0)
            {
                Console.WriteLine("No weapons in the inventory.");
                return;
            }

            Console.WriteLine("\n=== Weapons by Rarity ===");
            Console.WriteLine("".PadRight(50, '='));

            // Order weapons by quality (ascending)
            var orderedWeapons = weapons.OrderBy(w => w.Quality).ToList();

            foreach (Weapon w in orderedWeapons)
            {
                Console.ForegroundColor = GetRarityColor(w.Quality);
                Console.WriteLine(w.RarityDetails());
                Console.ResetColor();
                Console.WriteLine("".PadRight(50, '-'));
            }
        }

        // Helper method to determine color based on weapon quality
        static ConsoleColor GetRarityColor(int quality)
        {
            return quality switch
            {
                >= 100 => ConsoleColor.Yellow,    // Legendary
                >= 50 => ConsoleColor.DarkMagenta,          // Epic
                >= 20 => ConsoleColor.Cyan,            // Rare
                >= 10 => ConsoleColor.Blue,            // Uncommon
                >= 0 => ConsoleColor.Gray,            // Common
                < 0 => ConsoleColor.Red                // Cursed
            };
        }

        // Load weapons from CSV file with comprehensive exception handling
        static void LoadWeapons(List<Weapon> weapons)
        {
            try
            {
                // Use 'using' statement to automatically dispose of StreamReader
                using (StreamReader reader = new StreamReader(INVENTORY_FILE))
                {
                    string line;
                    // Read file line by line until end of file
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Convert each CSV line to a Weapon object
                        Weapon weapon = Weapon.FromCSVString(line);
                        weapons.Add(weapon);
                    }

                    GreenText("Weapons loaded from file.");
                }
            }
            catch (IOException ex)
            {
                // Handle file access or read errors
                RedText($"Error reading file: {ex.Message}");
            }
            catch (FormatException ex)
            {
                // Handle parsing errors when converting CSV data to Weapon objects
                RedText($"Error parsing data from file: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Generic error handling for unexpected exceptions
                RedText($"Unexpected error loading weapons: {ex.Message}");
            }
        }

        // Add a new weapon to inventory with validation
        static Weapon AddWeapon(List<Weapon> weapons)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
          _,.
        ,` -.)
       ( _/-\\-._
      /,|`--._,-^|            ,
      \_| |`-._/||          ,|
        |  `-, / |         /  /
        |     || |        /  /
         `r-._||/   __   /  /
     __,-<_     )`-/  `./  /
    '  \   `---'   \   /  /
        |           |./  /
        /           //  /
    \_/' \         |/  /
     |    |   _,^-'/  /
     |    , ``  (\/  /_
      \,.->._    \X-=/^
      (  /   `-._//^`
       `Y-.____(__}
        |     {__)
              ()");
            Console.ResetColor();
            // Variables to store weapon properties
            string type;
            int quality;
            double cost;
            int inventory;
            Weapon weapon = new Weapon();

            bool validType = false;

            // Get and validate weapon type, ensuring no duplicates
            do
            {
                try
                {
                    type = Prompt("Enter the weapon type: ").Trim();

                    // Check for duplicates before assigning
                    bool isDuplicate = false;
                    foreach (Weapon w in weapons)
                    {
                        if (w.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                        {
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (isDuplicate)
                    {
                        // Inform user if weapon type already exists
                        RedText("A weapon with this type already exists. Please use a different type.");
                        validType = false;
                    }
                    else
                    {
                        // Assign type if it's valid and unique
                        weapon.Type = type;
                        validType = true;
                    }
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors from the Weapon class
                    RedText(ex.Message);
                    validType = false;
                }
            } while (!validType);

            // Get and validate weapon quality
            bool validQuality = false;

            do
            {
                try
                {
                    quality = PromptInt("Enter the weapon quality (Can be a negative or a positive value): ");
                    weapon.Quality = quality;
                    validQuality = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors from the Weapon class
                    RedText(ex.Message);
                    validQuality = false;
                }
            } while (!validQuality);

            // Get and validate weapon cost
            bool validCost = false;

            do
            {
                try
                {
                    cost = PromptDouble("Enter the weapon's cost (Cannot be 0 or less than zero): ");
                    weapon.Cost = cost;
                    validCost = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors from the Weapon class
                    RedText(ex.Message);
                    validCost = false;
                }
            } while (!validCost);

            // Get and validate inventory amount
            bool validInventory = false;

            do
            {
                try
                {
                    inventory = PromptInt("Enter the quantity of weapons being added to the inventory (must be greater than or equal to 0): ");
                    weapon.AmountOfInventory = inventory;
                    validInventory = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors from the Weapon class
                    RedText(ex.Message);
                    validInventory = false;
                }
            } while (!validInventory);

            Console.WriteLine("QUIT TO SAVE CHANGES.");
            return weapon;
        }

        // Edit an existing weapon's properties
        static void EditWeapon(List<Weapon> weapons)
        {
            string search;
            Weapon found = null;

            bool validSearch = false;

            // Search for the weapon to edit
            do
            {
                search = Prompt("Enter the weapon type to edit: ");

                // Find the weapon in inventory
                foreach (Weapon w in weapons)
                {
                    if (w.Type.Equals(search, StringComparison.OrdinalIgnoreCase))
                    {
                        found = w;
                        validSearch = true;
                        break;
                    }
                }

                if (!validSearch)
                {
                    // Inform user if weapon not found
                    RedText("Weapon not found. Please try again.");
                }

            } while (!validSearch);

            Console.WriteLine("Editing: " + found.Type);

            // Validate and update Quality
            bool validQuality = false;
            do
            {
                try
                {
                    int quality = PromptInt("Enter the new weapon quality (Can be a negative or a positive value): ");
                    found.Quality = quality;
                    validQuality = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors
                    RedText(ex.Message);
                }
            } while (!validQuality);

            // Validate and update Cost
            bool validCost = false;
            do
            {
                try
                {
                    double cost = PromptDouble("Enter the weapon's new cost (Cannot be 0 or less than zero): ");
                    found.Cost = cost;
                    validCost = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors
                    RedText(ex.Message);
                }
            } while (!validCost);

            // Validate and update Inventory
            bool validInventory = false;
            do
            {
                try
                {
                    int inventory = PromptInt("\"Enter the edited quantity of weapons in the inventory (must be greater than or equal to 0): ");
                    found.AmountOfInventory = inventory;
                    validInventory = true;
                }
                catch (ArgumentException ex)
                {
                    // Handle validation errors
                    RedText(ex.Message);
                }
            } while (!validInventory);


            GreenText("Weapon updated successfully.");
            Console.WriteLine("QUIT TO SAVE CHANGES.");
        }

        // Search for a weapon by its type
        static void SearchWeapon(List<Weapon> weapons)
        {
            bool rightInput = false;
            do
            {
                string search = Prompt("Enter weapon type to search: ");
                Weapon found = null;

                // Search for the weapon in inventory
                foreach (Weapon w in weapons)
                {
                    if (w.Type.Equals(search, StringComparison.OrdinalIgnoreCase))
                    {
                        found = w;
                        break;
                    }
                }

                if (found == null)
                {
                    // Inform user if weapon not found
                    RedText("Weapon not found. Please try again.");
                    rightInput = false;
                }
                else
                {
                    // Display weapon details if found
                    Console.WriteLine(found.WeaponDetails());
                    rightInput = true;
                }

                // Ask if user wants to search for another weapon
                string again = Prompt("Do you want to search for another weapon? (yes/no): ").Trim().ToLower();
                if (again != "yes")
                {
                    break;
                }

            } while (rightInput == false);
        }

        // Remove a weapon from inventory
        static void RemoveWeapon(List<Weapon> weapons)
        {
            bool rightInput = false;
            do
            {
                // Show all weapons to help user select which to remove
                DisplayAllWeapons(weapons);

                string search = Prompt("Enter the weapon type to remove: ");
                Weapon found = null;

                // Search for the weapon to remove
                foreach (Weapon w in weapons)
                {
                    if (w.Type.Equals(search, StringComparison.OrdinalIgnoreCase))
                    {
                        found = w;
                        break;
                    }
                }

                if (found == null)
                {
                    // Inform user if weapon not found
                    RedText("Weapon not found. Please try again.");
                    rightInput = false;
                }
                else
                {
                    // Remove weapon if found
                    rightInput = true;
                    weapons.Remove(found);
                    GreenText("Weapon removed successfully.");
                }

                // Ask if user wants to remove another weapon
                string again = Prompt("Do you want to remove another weapon? (yes/no): ").Trim().ToLower();
                if (again != "yes")
                {
                    Console.WriteLine("QUIT TO SAVE CHANGES.");
                    break;
                    // Note: The "break" statement here makes the next line unreachable
                    // This is a potential logical error in the code
                }


            } while (rightInput == false);
        }

        // Helper method to prompt user for input
        static string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        // Helper method to prompt user for double value with validation
        static double PromptDouble(string message)
        {
            double number;
            bool isValid = false;

            do
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (double.TryParse(input, out number))
                {
                    isValid = true;
                }
                else
                {
                    // Inform user of invalid input
                    RedText("Invalid input. Please enter a valid number.");
                }

            } while (!isValid);

            return number;
        }

        // Helper method to prompt user for integer value with validation
        static int PromptInt(string message)
        {
            int number;
            bool isValid = false;

            do
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (int.TryParse(input, out number))
                {
                    isValid = true;
                }
                else
                {
                    // Inform user of invalid input
                    RedText("Invalid input. Please enter a valid number.");
                }

            } while (!isValid);

            return number;
        }

        // Display the main menu options
        // MODIFIED MENU DISPLAY WITH BETTER FORMATTING
        static void DisplayMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║         INVENTORY MANAGEMENT         ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ [D] Display Inventory                ║");
            Console.WriteLine("║ [S] Search Inventory                 ║");
            Console.WriteLine("║ [A] Add New Weapon                   ║");
            Console.WriteLine("║ [E] Edit Weapon                      ║");
            Console.WriteLine("║ [R] Remove Weapon                    ║");
            Console.WriteLine("║ [V] View Average Cost                ║");
            Console.WriteLine("║ [L] List Weapons by Rarity           ║");
            Console.WriteLine("║ [Q] Quit                             ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("Choose your option: ");
        }

        // Calculate and display the average cost of all weapons
        static void ViewAverageCost(List<Weapon> weapons)
        {
            // Check if inventory is empty
            if (weapons.Count == 0)
            {
                Console.WriteLine("No weapons in the inventory to calculate average cost.");
                return;
            }

            double totalCost = 0;
            int totalWeapons = 0;

            // Calculate total cost and count of all weapons
            foreach (Weapon w in weapons)
            {
                totalCost += w.Cost * w.AmountOfInventory;
                totalWeapons += w.AmountOfInventory;
            }

            // Check if there are actual weapon items (quantity) to calculate average
            if (totalWeapons == 0)
            {
                Console.WriteLine("No weapon items available to calculate average.");
                return;
            }

            // Calculate and display average cost
            double average = totalCost / totalWeapons;
            GreenText($"Average cost of all weapons in inventory: {average:F2}");
        }

        // Helper method to display text in red (for errors/warnings)
        static void RedText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Helper method to display text in green (for success messages)
        static void GreenText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
