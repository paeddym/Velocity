namespace Velocity {
    public class Core {
        static void Main(string[] args) {
            using (Game game = new Game(800, 600, "Learn OpenTK")) {
                game.Run();
            }
        }
    }
}
