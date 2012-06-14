using System;

namespace StarlingTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StarlingGame game = new StarlingGame())
            {
                game.Run();
            }
        }
    }
#endif
}

