namespace GameApp{
    public class Core {
        static void Main(string[] args) {
            using (Game game = new Game(800, 600, "Learn OpenTK")) {
                Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
                Console.WriteLine("Base Directory: " + AppContext.BaseDirectory);

                game.Run();
            }
        }
    }
}
