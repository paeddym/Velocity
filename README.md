# Velocity
A racing game written in C# using OpenTK

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

### Setup on Arch
- install the dotnet 8 framework from the arch arcive with 'pacman -U'
- start the session with x11 instead of Wayland as OpenGL is buggy with Wayland
- build an run with 'dotnet build' 'dotnet run' in this order at the root of the project where the '.sln and .csproj' life
