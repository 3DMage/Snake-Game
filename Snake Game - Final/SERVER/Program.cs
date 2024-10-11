namespace ServerConsole
{
    internal class Program
    {
        public static int Main(string[] args)
        {




            string Bens = @"

██████╗ ███████╗███╗   ██╗███████╗               
██╔══██╗██╔════╝████╗  ██║██╔════╝               
██████╔╝█████╗  ██╔██╗ ██║███████╗               
██╔══██╗██╔══╝  ██║╚██╗██║╚════██║               
██████╔╝███████╗██║ ╚████║███████║               
╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝        ";
            string Snake = @"
    ███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗           
    ██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝           
    ███████╗██╔██╗ ██║███████║█████╔╝ █████╗             
    ╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝             
    ███████║██║ ╚████║██║  ██║██║  ██╗███████╗           
    ╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝       ";

            string Server = @"
        ███████╗███████╗██████╗ ██╗   ██╗███████╗██████╗ 
        ██╔════╝██╔════╝██╔══██╗██║   ██║██╔════╝██╔══██╗
        ███████╗█████╗  ██████╔╝██║   ██║█████╗  ██████╔╝
        ╚════██║██╔══╝  ██╔══██╗╚██╗ ██╔╝██╔══╝  ██╔══██╗
        ███████║███████╗██║  ██║ ╚████╔╝ ███████╗██║  ██║
        ╚══════╝╚══════╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝
                                                 

";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Bens);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Snake);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Server);

            Console.ForegroundColor = ConsoleColor.Green;

            ushort port;

            if(ReadArgs(args, out port))
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                _Server.Initialize(cancellationTokenSource.Token, port);

                Thread serverThread = new Thread(_Server.Run);
                serverThread.Start();

                RunUntilStopCommand();

                cancellationTokenSource.Cancel();
                serverThread.Join();
            }
            else
            {
                port = 3000;

                Console.WriteLine("Invalid port parameter.  Using port 3000 instead");

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                _Server.Initialize(cancellationTokenSource.Token, port);

                Thread serverThread = new Thread(_Server.Run);
                serverThread.Start();

                RunUntilStopCommand();

                cancellationTokenSource.Cancel();
                serverThread.Join();
            }

            return 0;
        }


        public static void RunUntilStopCommand()
        {
            Console.WriteLine("Enter 'stop' to quit.");

            bool done = false;
            // Continuously read the input from the user until they type "stop".
            while (!done)
            {
                // Read user input from the console.
                string userInput = Console.ReadLine();

                // Check if the user input is "stop", ignoring case sensitivity.
                if (userInput != null && userInput.Equals("stop", StringComparison.OrdinalIgnoreCase))
                {
                    done = true;
                }
            }
        }






        private static bool ReadArgs(string[] args, out ushort port)
        {
            Predicate<string> PortParam = (arg => arg == "-p" || arg == "--port" || arg == "-port");

            port = 0;
            bool valid = true;
            if (args.Length != 2)
            {
                valid = false;
            }
            else if (!PortParam(args[0].ToLower()))
            {
                valid = false;
            }
            else
            {
                if (!ushort.TryParse(args[1], out port))
                {
                    valid = false;
                }
            }

            return valid;
        }



    }
}
