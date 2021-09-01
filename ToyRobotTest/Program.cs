using System;
using ToyRobotLib;

namespace ToyRobotTest
{
  class Program
  {

    static void Main()
    {
      // Initialise robot controller
      IRobotController controller = new RobotController();

      Console.WriteLine(controller.CommandInput("REPORT"));
      Console.WriteLine("Please enter a command or hit 'x' to exit:");

      string input = string.Empty;
      while (input.ToLowerInvariant() != "x")
      {
        if (!string.IsNullOrWhiteSpace(input))
        {
          Console.WriteLine(controller.CommandInput(input));
        }
        input = Console.ReadLine();
      }
    }
  }
}
