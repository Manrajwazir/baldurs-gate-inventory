Baldur's Gate Weapon Inventory Manager - README
Overview
This C# console application is designed to help merchants in Baldur's Gate manage their weapon inventory. The program allows users to add, edit, remove, search, and display weapons with various attributes including type, quality, cost, and inventory amount.

Features
Core Functionality
Inventory Management: Full CRUD (Create, Read, Update, Delete) operations for weapons

Data Persistence: Saves inventory to and loads from a CSV file (inventory.csv)

Detailed Weapon Information: Tracks type, quality, cost, inventory amount, and calculates total value

Rarity System: Automatically classifies weapons based on quality (Poor, Common, Rare, Epic, Legendary)

User Interface
ASCII Art: Welcome and goodbye screens with thematic ASCII art

Color-Coded Display: Weapons displayed in colors matching their rarity

Menu System: Intuitive console-based menu navigation

Special Features
Average Cost Calculation: Calculates the average cost of all weapons in inventory

Rarity Sorting: Displays weapons ordered by rarity level

Input Validation: Robust validation for all user inputs

Code Structure
Program.cs
The main application file containing:

Main Menu System: Handles all user interactions through a switch-case structure

File Operations: Methods for loading and saving weapon data

Display Methods: Functions for showing weapons with color-coded rarity

Input Helpers: Utility methods for prompting and validating user input

Error Handling: Comprehensive try-catch blocks for file operations and data processing

Weapon.cs
The class representing weapon items with:

Properties:

Type (string, required)

Quality (int, can be positive or negative)

Cost (double, must be ≥ 1)

AmountOfInventory (int, must be ≥ 0)

Calculated Properties:

ValueOfAllItems: Total value (cost × quantity)

Rarity: Automatic classification based on quality

Methods:

WeaponDetails(): Returns formatted string with all weapon info

RarityDetails(): Simplified display of type and rarity

ToCSVString(): Converts weapon to CSV format

FromCSVString(): Static method to create Weapon from CSV line

How to Use
Run the application to see the welcome screen

Use the menu options to:

Display all weapons

Search for specific weapons

Add new weapons

Edit existing weapons

Remove weapons

View average cost

List weapons by rarity

Quit (saves changes)

Follow on-screen prompts for each operation

Changes are automatically saved when quitting

Technical Details
File Storage: Uses CSV format for simplicity and readability

Validation: Comprehensive validation for all weapon properties

Error Handling: Graceful handling of file operations and user input

Color Coding: Visual distinction of weapon rarities in console output

Requirements
.NET Core/.NET 5+ runtime

Console environment that supports ANSI color codes

Notes
The application demonstrates:

Object-oriented programming principles

File I/O operations

Data validation

User interface design in console applications

Exception handling