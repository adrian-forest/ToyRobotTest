## Toy Robot Simulator
--------------------
#### Create a library that can read in commands of the following form:

- PLACE X,Y,DIRECTION
- MOVE
- LEFT
- RIGHT
- REPORT

1. The library allows for a simulation of a toy robot moving on a 6 x 6 square tabletop.
2. There are no obstructions on the table surface.
3. The robot is free to roam around the surface of the table, but must be prevented from falling to destruction. Any movement that would result in this must be prevented, however further valid movement commands must still be allowed.
4. PLACE will put the toy robot on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST.
5. (0,0) can be considered as the SOUTH WEST corner and (5,5) as the NORTH EAST corner.
6. The first valid command to the robot is a PLACE command. After that, any sequence of commands may be issued, in any order, including another PLACE command. The library should discard all commands in the sequence until a valid PLACE command has been executed.
7. The PLACE command should be discarded if it places the robot outside of the table surface.
8. Once the robot is on the table, subsequent PLACE commands could leave out the direction and only provide the coordinates. When this happens, the robot moves to the new coordinates without changing the direction.
9. MOVE will move the toy robot one unit forward in the direction it is currently facing.
10. LEFT and RIGHT will rotate the robot 90 degrees in the specified direction without changing the position of the robot.
11. REPORT will announce the X,Y and orientation of the robot.
12. A robot that is not on the table can choose to ignore the MOVE, LEFT, RIGHT and REPORT commands.
13. The library should discard all invalid commands and parameters.
