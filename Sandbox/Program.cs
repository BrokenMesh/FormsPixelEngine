namespace FormsPixelEngine
{
    internal static class Program
    {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();
            Application.Run(new Sandbox("Sandbox", 900,600, 300,200));
        }
    }
}