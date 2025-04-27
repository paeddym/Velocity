using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Velocity
{
    public class CollisionManager
    {
        private List<CubeGen> cubes;
        private Car car;

        public CollisionManager(Car car, List<CubeGen> cubes)
        {
            this.car = car;
            this.cubes = cubes;
        }

        public void CheckCollisions()
        {
            Vector3 carPos = car.GetPosition();
            Vector2 carSize = car.GetSize();

            foreach (var cube in cubes)
            {
                Vector3 cubePos = cube.GetPosition();
                Vector2 cubeSize = cube.GetSize();

                if (RectCollision(carPos, carSize, cubePos, cubeSize))
                {
                    HandleCollision(car, cube);
                }
            }
        }

        private bool RectCollision(Vector3 posA, Vector2 sizeA, Vector3 posB, Vector2 sizeB)
        {
            return (posA.X < posB.X + sizeB.X &&
                    posA.X + sizeA.X > posB.X &&
                    posA.Y < posB.Y + sizeB.Y &&
                    posA.Y + sizeA.Y > posB.Y);
        }

        private void HandleCollision(Car car, CubeGen cube)
        {
            Console.WriteLine("Collision Detected!");
            car.BounceBack();
        }
    }
}

