using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb44
{
    class Program
    {

        public static Monster monster1 = new Monster(5,5);
        public static Monster monster2 = new Monster(12, 12);
        public static Monster monster3 = new Monster(15, 2);



        public static int difficulty = 0;
        public static Player player = new Player();

        public static String[,] GameBoard = GenerateBoard();

        //Path for Highscore.txt
        public static string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
        static string SCORE_PATH = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName + @"\Highscore.txt";
       
        public static bool GameLoop = true;

        static void Main(string[] args)
        {
            Key key1 = new Key(12, 5);
            Key key2 = new Key(9, 4);
            Key key3 = new Key(12, 9);

            Key[] keys = { key1, key2, key3 };

            StartGame();
            Console.Clear();

            //Start of game loop
            while (GameLoop)
            {
                Random rnd = new Random();
                GetPlayerPosition();
                UpdateMonsterPosition(monster1, rnd.Next(1, 5));
                UpdateMonsterPosition(monster2, rnd.Next(1, 5));
                UpdateMonsterPosition(monster3, rnd.Next(1, 5));
                GetMonsterPosition(monster1);
                GetMonsterPosition(monster2);
                GetMonsterPosition(monster3);
                GetKeyPosition(keys);
                GetPlayerPosition();
                PrintMap();
                Console.WriteLine("Player has moved " + player.Steps + " steps");
                Console.WriteLine("Player has " + player.GetPlayerKeys() + " keys");

                //Control logic for movement with WASD
                var ch = Console.ReadKey(false).Key;
                switch (ch)
                {
                    case ConsoleKey.W:
                        UpdatePlayerPosition("W", keys);
                        break;
                    case ConsoleKey.D:
                        UpdatePlayerPosition("D", keys);
                        break;
                    case ConsoleKey.A:
                        UpdatePlayerPosition("A", keys);
                        break;
                    case ConsoleKey.S:
                        UpdatePlayerPosition("S", keys);
                        break;
                }
            }
        }

        private static void StartGame()
        {
            player.X = 2;
            player.Y = 2;
            Console.WriteLine("Welcome to the dungeon crawler.");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            while(name.Length <= 0)
            {
                Console.WriteLine("You must have a name");
                name = Console.ReadLine();

            }
            player.SetPlayerName(name);
            Console.WriteLine("Choose a difficulty: \n1. Easy (Monster stepping on you will cost you 20 extra steps)" +
                "\n2. Medium (Monster stepping on you will cost you extra 50 steps)" +
                "\n3. Hard (Monster will KILL you.)");

            while (!int.TryParse(Console.ReadLine(), out difficulty) || (difficulty <= 0) || (difficulty >= 4))
            {
                Console.WriteLine("That is not a valid choice.");
            }
        }


        //Gets the current player position and sets the GameBoards tile that matches the same position as player to a player tile("@").
        public static void GetPlayerPosition()
        {
            GameBoard[player.Y, player.X] = player.Tile;
        }
        /* UpdatePlayerPosition() updates the players y and x coordinate. For example pressing the "D" key will move the player right by incrementing the x coordinate.
         * We have to replace the players current coordinate with a floor tile and then move the player.
         */
        public static void GetKeyPosition(Key[] keys)
        {
            foreach (Key key in keys)
            {
                if (key.IsFound == false)
                {
                    GameBoard[key.Y, key.X] = key.Tile;
                }
            }
        }

        //Gets the current monster position and sets the GameBoards tile that matches the same position as player to a monster tile("m").
        public static void GetMonsterPosition(Monster monster)
        {
            GameBoard[monster.Y, monster.X] = monster.Tile;
        }

        //Since we know the direction the entity is moving we can see if that direction is valid with help of the MonsterCollision func.
        //We basicaly do nothing if the collision check doesnt pass.
        public static void UpdateMonsterPosition(Monster monster, int monsterdirection)
        {
            Floor floor = new Floor();
        
            if(monsterdirection == 1)
            {
                if (MonsterCollision(monster.X + 1, monster.Y))
                {
                    GameBoard[monster.Y, monster.X] = floor.Tile;
                    monster.X += 1;
                }
            }
            if (monsterdirection == 2)
            {
                if (MonsterCollision(monster.X, monster.Y + 1))
                {
                    GameBoard[monster.Y, monster.X] = floor.Tile;
                    monster.Y += 1;
                }
            }
            if (monsterdirection == 3)
            {
                if (MonsterCollision(monster.X, monster.Y - 1))
                {
                    GameBoard[monster.Y, monster.X] = floor.Tile;
                    monster.Y -= 1;
                }
            }
            if (monsterdirection == 4)
            {
                if (MonsterCollision(monster.X - 1, monster.Y))
                {
                    GameBoard[monster.Y, monster.X] = floor.Tile;
                    monster.X -= 1;
                }
            }
            if(monster.X == player.X && monster.Y == player.Y)
            {
                if(difficulty == 1)
                {
                    player.Steps += 20; 
                } else if(difficulty == 2)
                {
                    player.Steps += 50;
                } else
                //Game over case
                {
                    Console.Clear();
                    Console.SetCursorPosition(22, 5);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("YOU DIED");
                    Console.ResetColor();
                    GameLoop = false;
                }
            }
        }

        //Checks the tile of the given coordinate.
        public static bool MonsterCollision(int x, int y)
        {
            Wall wall = new Wall();
            Door door = new Door();
            HorisontalDoor hdoor = new HorisontalDoor();
            Key hiddenKey = new Key(0, 0);
            Monster hiddenMonster = new Monster(0, 0);
            Player player = new Player();

            if (GameBoard[y, x].Equals(wall.Tile))
            {
                return false;
            }
            if (GameBoard[y, x].Equals(door.Tile))
            {
                return false;
            }
            if (GameBoard[y, x].Equals(hdoor.Tile))
            {
                return false;
            }
            if (GameBoard[y, x].Equals(hiddenKey.Tile))
            {
                return false;
            }
            if (GameBoard[y, x].Equals(player.Tile))
            {
                return true;
            }
            if (GameBoard[y, x].Equals(hiddenMonster.Tile))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Since we know the direction the entity is moving we can see if that direction is valid with help of the Collision func.
        //We basicaly do nothing if the collision check doesnt pass.
        public static void UpdatePlayerPosition(string direction, Key[] keys)
        {
            Floor floor = new Floor();
            switch (direction)
            {
                case "D":
                    //Collision checks if player is able to move to desired destination, and moves if its true.
                    if (Collision(player.X + 1, player.Y))
                    {
                        GameBoard[player.Y, player.X] = floor.Tile;
                        player.X += 1;
                        player.Steps++;
                    }
                    break;
                case "S":
                    if (Collision(player.X, player.Y + 1))
                    {
                        GameBoard[player.Y, player.X] = floor.Tile;
                        player.Y += 1;
                        player.Steps++;
                    }
                    break;
                case "W":
                    if (Collision(player.X, player.Y - 1))
                    {
                        GameBoard[player.Y, player.X] = floor.Tile;
                        player.Y -= 1;
                        player.Steps++;
                    }
                    break;
                case "A":
                    if (Collision(player.X - 1, player.Y))
                    {
                        GameBoard[player.Y, player.X] = floor.Tile;
                        player.X -= 1;
                        player.Steps++;
                    }
                    break;
                default:
                    break;
            }
            
            //Scans for keys on player coordinate.
            foreach(Key key in keys)
            {
                if(player.X == key.X && player.Y == key.Y && key.IsFound == false)
                {
                    player.AddPlayerKey();
                    key.IsFound = true;
                }
            }

            
        }
        
        //All types of collision will go here. Returns a bool and uses it in the UpdatePlayerPosition function to see if the move is
        //a valid move or not.
        public static bool Collision(int x, int y)
        {
            Wall wall = new Wall();
            Door door = new Door();
            Floor floor = new Floor();
            Exit exit = new Exit();
            HorisontalDoor hdoor = new HorisontalDoor();

            bool collision = true;

            if(GameBoard[y, x].Equals(wall.Tile))
            {
                collision = false;
            }
            if (GameBoard[y, x].Equals(door.Tile) && player.GetPlayerKeys() == 0)
            {
                collision = false;
            }
            else if(GameBoard[y, x].Equals(door.Tile))
            {
                player.RemovePlayerKey();
            }
            if (GameBoard[y, x].Equals(hdoor.Tile) && player.GetPlayerKeys() == 0)
            {
                collision = false;
            } else if(GameBoard[y, x].Equals(hdoor.Tile))
            {
                player.RemovePlayerKey();
            }

            if (GameBoard[y, x].Equals(monster1.Tile))
            {
                //player.Steps += 20;
                collision = false;
            }
            if(GameBoard[y, x].Equals(exit.Tile))
            {
                EndGame(player);
                GameLoop = false;
                collision = true;
            }
            return collision;
        }
        public static void EndGame(Player player)
        {
            if (File.Exists(SCORE_PATH))
            {
                using (StreamWriter sw = File.AppendText(SCORE_PATH))
                {
                    int steps = player.Steps;
                    string name = player.GetPlayerName();
                    sw.WriteLine(steps + "," + name);
                }
            }
            Console.Clear();
            Console.WriteLine($"Congratulations you won! You took {player.Steps} number of steps, check below to see how you placed against other players.");
            Console.WriteLine("HIGHSCORE: ");
            ReturnHighscore();
        }

        
        //Reads the SCORE_PATH variable and parses the content so it can be displayed accordingly.
        private static void ReturnHighscore()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            if (File.Exists(SCORE_PATH))
            {
                string[] lines = File.ReadAllLines(SCORE_PATH);
                foreach (var line in lines)
                {
                    string[] items = line.Split(',');
                    list.Add(new KeyValuePair<int, string>(int.Parse(items[0]), items[1].ToString()));
                }
                ;
                foreach (var score in list.OrderBy(s => s.Key))
                {
                    Console.WriteLine(score);
                }
                Console.ReadKey();
            }
            else
            {
                //empty list
                Console.WriteLine("Missing highscore file, check file path on SCORE_PATH variable");
                Console.ReadKey();
            }
        }

        //Generates a map based on the rawMap multidimensional array.
        private static string[,] GenerateBoard()
        {
            Wall wall = new Wall();
            Floor floor = new Floor();
            Door door = new Door();
            HorisontalDoor hdoor = new HorisontalDoor();
            Exit exit = new Exit();
            string[,] rawMap = new string[,] {    { wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile,wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, },
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, wall.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, wall.Tile, floor.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, door.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, wall.Tile, wall.Tile, hdoor.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, },
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, exit.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, door.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, floor.Tile, wall.Tile},
                                                        { wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile, wall.Tile,},
                                                        
            };
            return rawMap;
        }

        //Outputs the map to console in a grid style layout where y represents the vertical line and x represents the horizontal
        public static void PrintMap()
        {
            Console.SetCursorPosition(0, 0);
            string map = "";
            for (int y = 0; y < GameBoard.GetLength(0); y++)
            {
                for (int x = 0; x < GameBoard.GetLength(1); x++)
                {
                    map += GameBoard[y, x];
                }
                map += '\n';
            }
            Console.Write(map);
        }
    }
}
