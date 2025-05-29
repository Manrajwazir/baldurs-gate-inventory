using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment4ManrajSinghWazir
{
    // Represents a weapon in the inventory system
    // Implements properties with validation to ensure data integrity
    public class Weapon
    {
        // Backing fields for properties - private members
        private string _type;
        private int _quality;
        private double _cost;
        private int _amountOfInventory;

        // Type property - validates that weapon type is not empty
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                // Validation: check if the value is null, empty or whitespace
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Type cannot be null, empty or whitespace.");
                }
                _type = value.Trim(); // Store trimmed version to remove any leading/trailing spaces
            }
        }

        // Quality property - can be positive or negative to represent good or poor quality
        public int Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                // No validation needed as both positive and negative values are allowed
                _quality = value;
            }
        }

        // Cost property - enforces minimum value of 1
        public double Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                // Validation: check if the value is less than minimum allowed
                if (value < 1)
                {
                    throw new ArgumentException("The cost must be greater than or equal to 1");
                }
                _cost = value;
            }
        }

        // AmountOfInventory property - enforces non-negative values
        public int AmountOfInventory
        {
            get
            {
                return _amountOfInventory;
            }
            set
            {
                // Validation: check if the value is negative
                if (value < 0)
                {
                    throw new ArgumentException("The amount in inventory must be greater than or equal to 0");
                }
                _amountOfInventory = value;
            }
        }

        // Calculated property: total value of all items of this weapon type
        public double ValueOfAllItems
        {
            get
            {
                return _cost * _amountOfInventory; // Formula: cost per item * quantity
            }
        }

        // Calculated property: determines rarity based on quality value
        public string Rarity
        {
            get
            {
                // Implement rarity tiers based on quality
                if (_quality < 1)
                {
                    return "Poor";
                }

                else if (_quality < 20)
                {
                    return "Common";
                }

                else if (_quality < 50)
                {
                    return "Rare";
                }

                else if (_quality < 100)
                {
                    return "Epic";
                }

                else
                {
                    return "Legendary"; // Quality 100 or higher
                }
            }
        }

        // Returns a formatted string with all weapon details
        public string WeaponDetails()
        {
            Console.WriteLine(); // Add blank line for readability
            string details =
                "WEAPON DETAILS:\n" +
                $"{"Type:",-20} {Type}\n" +                     // Left-aligned with 20 character width
                $"{"Quality:",-20} {Quality}\n" +               // Same formatting for all fields
                $"{"Cost Per Item:",-20} {Cost}\n" +
                $"{"Amount in Inventory:",-20} {AmountOfInventory}\n" +
                $"{"Rarity:",-20} {Rarity}\n" +
                $"{"Value of all items:",-20} {ValueOfAllItems}";

            return details;
        }

        // Returns a simplified format with just type and rarity
        public string RarityDetails()
        {
            Console.WriteLine(); // Add blank line for readability
            string details =
                "RARITY DETAILS:\n" +
                $"{"Type:",-20} {Type}\n" +    // Left-aligned with 20 character width
                $"{"Rarity:",-20} {Rarity}\n"; // Same formatting

            return details;
        }

        // Converts the weapon to a CSV string format for file storage
        public string ToCSVString()
        {
            return $"\"{Type}\",{Quality},{Cost},{AmountOfInventory}"; // Type in quotes to handle commas in names
        }

        // Static method to create a Weapon object from a CSV string
        public static Weapon FromCSVString(string csvLine)
        {
            string[] parts = csvLine.Split(','); // Split by comma delimiter

            // Validate CSV format
            if (parts.Length != 4)
            {
                throw new FormatException("CSV line is not in the correct format.");
            }

            // Parse each field, removing quotes from type and trimming whitespace
            string type = parts[0].Trim().Trim('"');
            int quality = int.Parse(parts[1]);
            double cost = double.Parse(parts[2]);
            int amount = int.Parse(parts[3]);

            // Create and return a new Weapon object
            return new Weapon(type, quality, cost, amount);
        }

        // Override ToString method for simplified output
        public override string ToString()
        {
            return string.Format("{0,-10} {1,-10} {2,-10} {3,-10}",
                                 Type, Quality, Cost, AmountOfInventory); // Fixed-width columns
        }

        // Default constructor
        public Weapon()
        {
            // Empty default constructor - properties will use default values
        }

        // Parameterized constructor (greedy constructor)
        public Weapon(string type, int quality, double cost, int amountOfInventory)
        {
            // Use properties for assignment to leverage validation
            Type = type;
            Quality = quality;
            Cost = cost;
            AmountOfInventory = amountOfInventory;
        }
    }
}
