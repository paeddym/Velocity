# Velocity
A racing game written in C# using OpenTK

# Controls
- B: Enable driving
- WASD: Steering
- Esc: Leave game

# TODO
## Compulsory
- Create a movable object
- Make it seem like steering
- Create a track to drive on -> Collisions
- Timer
- Add textures
- Add particles

## Optional
- AI enemies
- Multiple racetracks
- Items system
- Dynamic events on racetrack
- System to handle best times

### Setup on Windows
- Install Visual Studio 2022 and .NET 8 Framework
- Install the necessary NuGet packages (if not done automatically)
- Press Play Button in IDE
- If build not possible because asset/texture is missing: Right click on texture -> Properties | Build Action: Content | Copy to Outout Directory: Copy if newer

### Setup on Arch
- install the dotnet 8 framework from the arch arcive with 'pacman -U'
- start the session with x11 instead of Wayland as OpenGL is buggy with Wayland
- build an run with 'dotnet build' 'dotnet run' in this order at the root of the project where the '.sln and .csproj' life
