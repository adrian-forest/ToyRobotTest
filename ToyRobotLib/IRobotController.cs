namespace ToyRobotLib
{
  public interface IRobotController
  {
    /// <summary>
    /// Parses string input for valid command, and modifies the robot's position and/or facing if 
    /// the command is valid.
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <returns>The robot's current position and facing after the input is parsed.</returns>
    string CommandInput(string command);
  }
}