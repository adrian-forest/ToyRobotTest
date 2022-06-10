using System.Collections.Generic;

namespace ToyRobotLib
{
  public class RobotController : IRobotController
  {
    // Struct to hold the x and y values of the robot's position
    private struct Coordinates
    {
      private int x;
      private int y;

      public Coordinates(int newX, int newY)
      {
        x = newX;
        y = newY;
      }

      public int X { readonly get => x; set => x = value; }
      public int Y { readonly get => y; set => y = value; }
    }

    // Enum to provide a fixed set of possible directions
    private enum Directions
    {
      Invalid = 0,
      North = 1,
      East = 2,
      South = 3,
      West = 4,
    }

    // The size of the grid the robot is on
    private const int GRID_SIZE = 6;

    // Valid strings to parse as directions
    private static readonly List<string> validDirections = new()
    {
      "NORTH",
      "SOUTH",
      "EAST",
      "WEST",
    };

    // Valid inputs to parse as commands
    private static readonly List<string> validCommands = new()
    {
      "PLACE",
      "MOVE",
      "LEFT",
      "RIGHT",
      "REPORT",
    };

    // The robot's current position on the grid
    private Coordinates position = new() { X = -1, Y = -1 };

    // The robot's current facing on the grid
    private Directions currentFacing;

    // Whether the robot's position has been initialised with a PLACE command
    private bool robotPlaced = false;

    public RobotController()
    {
    }


    /// <summary>
    /// Parses string input for valid command, and modifies the robot's position and/or facing if 
    /// the command is valid.
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <returns>The robot's current position and facing after the input is parsed.</returns>
    public string CommandInput(string command)
    {
      command = command.ToUpperInvariant();
      if (command.StartsWith("PLACE"))
      {
        return Place(GetLocation(command), GetDirection(command));
      }

      if (!robotPlaced)
      {
        return Report();
      }

      if (!validCommands.Contains(command))
      {
        return null;
      }

      return command switch
      {
        "MOVE" => Move(),
        "LEFT" => Rotate(-1),
        "RIGHT" => Rotate(1),
        "REPORT" => Report(),
        _ => Report(),
      };
    }

    /// <summary>
    /// Helper function to test if a given set of coordinates is within the grid
    /// </summary>
    /// <param name="newPosition">The position to test</param>
    /// <returns>Whether the given coordinates are within the grid.</returns>
    private static bool ValidPosition(Coordinates newPosition)
    {
      return newPosition.X >= 0 && newPosition.X < GRID_SIZE
        && newPosition.Y >= 0 && newPosition.Y < GRID_SIZE;
    }

    /// <summary>
    /// Helper function to get a direction from a PLACE command
    /// </summary>
    /// <param name="command">The input string</param>
    /// <returns>The direction found in the command</returns>
    private static Directions GetDirection(string command)
    {
      // Get the direction from the characters after the last comma
      return command.Substring(command.LastIndexOf(',') + 1) switch
      {
        "NORTH" => Directions.North,
        "SOUTH" => Directions.South,
        "EAST" => Directions.East,
        "WEST" => Directions.West,
        _ => Directions.Invalid,
      };
    }

    /// <summary>
    /// Helper function to get a set of coordinates from a PLACE command
    /// </summary>
    /// <param name="command">The input string</param>
    /// <returns>The coordinates found in the command</returns>
    private static Coordinates GetLocation(string command)
    {
      Coordinates newPosition = new() { X = -1, Y = -1 };

      // Set X value from command by finding a number between the first space and first comma
      var firstSpacePos = command.IndexOf(' ') + 1;
      var firstCommaPos = command.IndexOf(',');

      // Test to ensure one or more characters can be extracted
      if (firstSpacePos < 0 || firstCommaPos < 0 || firstCommaPos < firstSpacePos)
      {
        return newPosition;
      }

      string xValue = command.Substring(firstSpacePos, firstCommaPos - firstSpacePos);

      if (string.IsNullOrWhiteSpace(xValue))
      {
        return newPosition;
      }

      if (int.TryParse(xValue, out int newX))
      {
        newPosition.X = newX;
      }
      else
      {
        return newPosition;
      }

      // Set Y value from command by finding a number between the first and last commas
      var lastCommaPos = command.LastIndexOf(',');

      string yValue = lastCommaPos > firstCommaPos
          ? command.Substring(
          firstCommaPos + 1,
          lastCommaPos - (firstCommaPos + 1))
          : command.Substring(firstCommaPos + 1);

      if (int.TryParse(yValue, out int newY))
      {
        newPosition.Y = newY;
      }

      return newPosition;
    }

    /// <summary>
    /// Rotates the robot through the cardinal directions
    /// </summary>
    /// <returns>The robot's current position and facing after rotation is applied.</returns>
    private string Rotate(int direction)
    {
      if (currentFacing == Directions.North && direction < 0)
      {
        currentFacing = Directions.West;
      }
      else if (currentFacing == Directions.West && direction > 0)
      {
        currentFacing = Directions.West;
      }
      else
      {
        currentFacing += direction;
      }

      return Report();
    }

    /// <summary>
    /// Modifies the robot's position within the bounds of the grid.
    /// </summary>
    /// <returns>The robot's current position and facing after the move is executed.</returns>
    private string Move()
    {
      Coordinates newPosition = position;

      switch (currentFacing)
      {
        case Directions.North:
          newPosition.Y++;
          break;
        case Directions.South:
          newPosition.Y--;
          break;
        case Directions.East:
          newPosition.X++;
          break;
        case Directions.West:
          newPosition.X--;
          break;
        default:
          break;
      }

      if (ValidPosition(newPosition))
      {
        position = newPosition;
      }

      return Report();
    }

    /// <summary>
    /// Sets the robot's position and facing if valid inputs for both are provided. If the robot is
    /// already placed the facing is not required to be valid
    /// </summary>
    /// <param name="initialPosition">The coordinates to place the robot at</param>
    /// <param name="initialFacing">The direction to place the robot facing</param>
    /// <returns>The robot's current position and facing after the command is executed.</returns>
    private string Place(Coordinates initialPosition, Directions initialFacing)
    {
      if (!ValidPosition(initialPosition))
      {
        return Report();
      }

      if (initialFacing == Directions.Invalid && !robotPlaced)
      {
        return Report();
      }

      position = initialPosition;

      if (initialFacing != Directions.Invalid)
      {
        currentFacing = initialFacing;
      }

      robotPlaced = true;

      return Report();
    }

    /// <summary>
    /// Reports the robot's current position and facing
    /// </summary>
    /// <returns>The robot's current position and facing</returns>
    private string Report()
    {
      if (!robotPlaced)
      {
        return "No robot placed.";
      }

      return "Robot is at " + position.X + ", " + position.Y + ", facing " + currentFacing.ToString();
    }
  }
}
