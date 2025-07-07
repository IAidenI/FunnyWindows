using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGame
{
    public struct Player
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum RetCode
    {
        FALSE = 0,
        TRUE = 1,
        EXIT = 3,
        LOOT = 4
    }

    public class Maze
    {
        // Dimenssion de la game
        private const int BLOCK_SIZE = 30;
        private int REAL_GAME_WIDTH = 0;
        private int REAL_GAME_HEIGHT = 0;

        private const int GAME_WIDTH = 29;
        private const int GAME_HEIGHT = 19;

        // Constant du labyrinthe
        private const int WALL = 0;
        private const int EXIT = 1;
        private const int PATH = 2;
        private const int VOID = 3;
        private const int FEXT = 4; // Fake exit
        private const int LOOT = 5; // Coffre
        private const int OPEN_LOOT = 6;

        private Point mouse_ref = new Point();
        public Point player { get; set; }
        public Point checkpoint { get; set; }

        private enum Direction
        {
            LEFT = 0,
            RIGHT = 1,
            UP = 2,
            DOWN = 3
        }

        private int[,] maze = new int[GAME_HEIGHT, GAME_WIDTH]
        {
            {WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,FEXT,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL},
            {WALL,VOID,PATH,PATH,PATH,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,PATH,VOID,PATH,PATH,PATH,VOID,PATH,PATH,PATH,PATH,VOID,VOID,PATH,PATH,PATH,WALL},
            {WALL,VOID,PATH,VOID,VOID,VOID,PATH,PATH,PATH,PATH,PATH,PATH,VOID,PATH,VOID,VOID,PATH,VOID,VOID,PATH,VOID,VOID,PATH,VOID,VOID,LOOT,VOID,PATH,WALL},
            {WALL,PATH,PATH,PATH,PATH,PATH,PATH,VOID,PATH,VOID,VOID,PATH,PATH,PATH,PATH,VOID,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,VOID,VOID,VOID,PATH,WALL},
            {WALL,PATH,VOID,VOID,VOID,VOID,PATH,VOID,PATH,VOID,VOID,PATH,VOID,VOID,VOID,VOID,PATH,VOID,VOID,VOID,VOID,VOID,VOID,PATH,PATH,PATH,PATH,PATH,WALL},
            {WALL,PATH,VOID,LOOT,PATH,VOID,PATH,VOID,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,LOOT,VOID,PATH,PATH,PATH,PATH,VOID,PATH,VOID,VOID,VOID,VOID,WALL},
            {WALL,PATH,VOID,VOID,PATH,VOID,PATH,VOID,VOID,VOID,VOID,PATH,VOID,VOID,VOID,VOID,PATH,VOID,PATH,VOID,VOID,PATH,PATH,PATH,PATH,PATH,PATH,VOID,WALL},
            {WALL,PATH,PATH,PATH,PATH,VOID,PATH,PATH,PATH,PATH,PATH,PATH,VOID,VOID,PATH,PATH,PATH,VOID,PATH,VOID,VOID,VOID,VOID,VOID,PATH,VOID,VOID,VOID,WALL},
            {WALL,VOID,VOID,VOID,VOID,VOID,VOID,PATH,VOID,VOID,VOID,PATH,VOID,VOID,PATH,VOID,PATH,VOID,PATH,VOID,VOID,PATH,PATH,PATH,PATH,VOID,PATH,PATH,WALL},
            {WALL,PATH,PATH,VOID,PATH,PATH,PATH,PATH,VOID,LOOT,PATH,PATH,PATH,VOID,PATH,VOID,PATH,PATH,PATH,PATH,VOID,VOID,PATH,VOID,VOID,VOID,VOID,PATH,WALL},
            {WALL,VOID,PATH,VOID,PATH,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,LOOT,PATH,PATH,PATH,PATH,PATH,PATH,VOID,PATH,WALL},
            {WALL,VOID,PATH,PATH,PATH,VOID,VOID,PATH,PATH,PATH,PATH,VOID,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,VOID,VOID,VOID,VOID,VOID,PATH,VOID,PATH,WALL},
            {WALL,VOID,VOID,VOID,VOID,VOID,VOID,PATH,VOID,VOID,PATH,VOID,PATH,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,VOID,PATH,PATH,PATH,PATH,PATH,WALL},
            {WALL,VOID,VOID,VOID,PATH,PATH,PATH,PATH,VOID,PATH,PATH,VOID,PATH,VOID,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,PATH,VOID,VOID,VOID,VOID,WALL},
            {WALL,VOID,PATH,PATH,PATH,VOID,PATH,VOID,VOID,PATH,VOID,VOID,PATH,PATH,PATH,VOID,PATH,VOID,VOID,VOID,PATH,VOID,VOID,PATH,VOID,PATH,PATH,PATH,WALL},
            {EXIT,PATH,PATH,VOID,VOID,VOID,PATH,VOID,VOID,PATH,PATH,PATH,PATH,VOID,VOID,VOID,PATH,PATH,PATH,VOID,PATH,VOID,VOID,PATH,VOID,LOOT,VOID,PATH,WALL},
            {WALL,VOID,VOID,VOID,PATH,PATH,PATH,PATH,VOID,VOID,PATH,VOID,PATH,VOID,LOOT,VOID,PATH,VOID,PATH,VOID,PATH,PATH,PATH,PATH,VOID,VOID,VOID,PATH,WALL},
            {WALL,LOOT,PATH,PATH,PATH,VOID,VOID,PATH,VOID,VOID,PATH,VOID,VOID,VOID,PATH,PATH,PATH,VOID,PATH,VOID,VOID,VOID,VOID,PATH,PATH,PATH,PATH,PATH,WALL},
            {WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,FEXT,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL,WALL},
        };
         
         
        public Maze(Point mouse_ref, Size size)
        {
            REAL_GAME_WIDTH = size.Width;
            REAL_GAME_HEIGHT = size.Height;
            this.mouse_ref = mouse_ref;

            this.player = realToGrid(this.mouse_ref);
            this.checkpoint = this.player;
        }

        public void reset(Point mouse_ref)
        {
            this.mouse_ref = mouse_ref;

            this.player = realToGrid(this.mouse_ref);
            this.checkpoint = this.player;
        }

        public RetCode isInPath(Point mouse_position)
        {
            if (mouse_position.X <= 0) return isExit(mouse_position, Direction.LEFT);
            if (mouse_position.Y <= 0) return isExit(mouse_position, Direction.UP);
            if (mouse_position.X >= REAL_GAME_WIDTH - 1) return isExit(mouse_position, Direction.RIGHT); // Le -1 très important, car l'indice est 0, donc si REAL_GAME_WIDTH fait 800 alors il sera entre 0 et 799
            if (mouse_position.Y >= REAL_GAME_HEIGHT - 1) return isExit(mouse_position, Direction.DOWN); // Idem

            // Si on atteint les limites du bloc
            if (mouse_position.X >= mouse_ref.X + BLOCK_SIZE / 2)
            {
                // On vérifie si le bloc suivant est libre ou non
                Console.WriteLine("mouse position : " + mouse_position);
                int value = maze[player.Y, player.X + 1];
                Console.Write("[ RIGHT ] maze : " + value + " player : " + player);
                if (value == VOID || value == WALL || value == FEXT) return RetCode.FALSE; // Si non libre alors retour au début
                else if (value == LOOT)
                {
                    maze[player.Y, player.X + 1] = OPEN_LOOT; // Marqué comme ouvert
                    return RetCode.LOOT; // Si c'est un coffre alors récupèrer le loot
                }
                else
                {
                    // Sinon on passe au bloc suivant
                    mouse_ref.X += BLOCK_SIZE;
                    Point s = player;
                    s.X += 1;
                    player = s;
                    Console.Write(" new player : " + player);
                }
            }
            else if (mouse_position.X <= mouse_ref.X - BLOCK_SIZE / 2)
            {
                // On vérifie si le bloc suivant est libre ou non
                Console.WriteLine("mouse position : " + mouse_position);
                int value = maze[player.Y, player.X - 1];
                Console.Write("[ LEFT ] maze : " + value + " player : " + player);
                if (value == VOID || value == WALL || value == FEXT) return RetCode.FALSE; // Si non libre alors retour au début
                else if (value == LOOT)
                {
                    maze[player.Y, player.X - 1] = OPEN_LOOT; // Marqué comme ouvert
                    return RetCode.LOOT; // Si c'est un coffre alors récupèrer le loot
                }
                else
                {
                    // Sinon on passe au bloc suivant
                    mouse_ref.X -= BLOCK_SIZE;
                    Point s = player;
                    s.X -= 1;
                    player = s;
                    Console.Write(" new player : " + player);
                }
            }
            else if (mouse_position.Y >= mouse_ref.Y + BLOCK_SIZE / 2)
            {
                // On vérifie si le bloc suivant est libre ou non
                Console.WriteLine("mouse position : " + mouse_position);
                int value = maze[player.Y + 1, player.X];
                Console.Write("[ DOWN ] maze : " + value + " player : " + player);
                if (value == VOID || value == WALL || value == FEXT) return RetCode.FALSE; // Si non libre alors retour au début
                else if (value == LOOT)
                {
                    maze[player.Y + 1, player.X] = OPEN_LOOT; // Marqué comme ouvert
                    return RetCode.LOOT; // Si c'est un coffre alors récupèrer le loot
                }
                else
                {
                    // Sinon on passe au bloc suivant
                    mouse_ref.Y += BLOCK_SIZE;
                    Point s = player;
                    s.Y += 1;
                    player = s;
                    Console.WriteLine(" new player : " + player);
                }
            }
            else if (mouse_position.Y <= mouse_ref.Y - BLOCK_SIZE / 2)
            {
                // On vérifie si le bloc suivant est libre
                Console.WriteLine("mouse position : " + mouse_position);
                int value = maze[player.Y - 1, player.X];
                Console.Write("[ UP ] maze : " + value + " player : " + player);
                if (value == VOID || value == WALL || value == FEXT) return RetCode.FALSE; // Si non libre alors retour au début
                else if (value == LOOT)
                {
                    maze[player.Y - 1, player.X] = OPEN_LOOT; // Marqué comme ouvert
                    return RetCode.LOOT; // Si c'est un coffre alors récupèrer le loot
                }
                else
                {
                    // Sinon on passe au bloc suivant
                    mouse_ref.Y -= BLOCK_SIZE;
                    Point s = player;
                    s.Y -= 1;
                    player = s;
                    Console.WriteLine(" new player : " + player);
                }
            }
            return RetCode.TRUE;
        }

        private RetCode isExit(Point mouse_position, Direction direction)
        {
            // En fonction de la direction que prend l'utilisateur on vérifie si le bloc suivant est la sortie
            int value = 0;
            switch (direction)
            {
                case Direction.LEFT:
                    value = maze[player.Y, player.X - 1];
                    break;
                case Direction.RIGHT:
                    value = maze[player.Y, player.X + 1];
                    break;
                case Direction.UP:
                    value = maze[player.Y - 1, player.X];
                    break;
                case Direction.DOWN:
                    value = maze[player.Y + 1, player.X];
                    break;
            }

            return value == EXIT ? RetCode.EXIT : RetCode.FALSE;
        }

        public Point getSafePlace()
        {
            List<Point> validPositions = new List<Point>();

            for (int y = 1; y < GAME_HEIGHT - 1; y++)
            {
                for (int x = 1; x < GAME_WIDTH - 1; x++)
                {
                    int value = maze[y, x];
                    if (value != WALL && value != VOID && value != FEXT && value != LOOT)
                    {
                        validPositions.Add(new Point(x, y));
                    }
                }
            }

            if (validPositions.Count == 0)
                throw new Exception("Aucune position valide trouvée.");

            Random rng = new Random();

            // Conversion de la grille en réel
            player = validPositions[rng.Next(validPositions.Count)];
            mouse_ref = gridToReal(player);

            int offsetX = (REAL_GAME_WIDTH - GAME_WIDTH * BLOCK_SIZE) / 2;
            int offsetY = (REAL_GAME_HEIGHT - GAME_HEIGHT * BLOCK_SIZE) / 2;

            int realX = offsetX + player.X * BLOCK_SIZE + BLOCK_SIZE / 2;
            int realY = offsetY + player.Y * BLOCK_SIZE + BLOCK_SIZE / 2;
            return new Point(realX, realY);
        }

        // Converti la grille réel dans le réferentiel de la grille du backend
        public Point realToGrid(Point real_pos)
        {
            // Parce qu'il peut y avoir des petites marges qui fait que start se trouve au mauvais endroit
            int offsetX = (REAL_GAME_WIDTH - GAME_WIDTH * BLOCK_SIZE) / 2;
            int offsetY = (REAL_GAME_HEIGHT - GAME_HEIGHT * BLOCK_SIZE) / 2;

            int caseX = (real_pos.X - offsetX) / BLOCK_SIZE;
            int caseY = (real_pos.Y - offsetY) / BLOCK_SIZE;

            // Clamp dans les limites de la grille au cas où
            caseX = Math.Max(0, Math.Min(GAME_WIDTH - 1, caseX));
            caseY = Math.Max(0, Math.Min(GAME_HEIGHT - 1, caseY));

            return new Point(caseX, caseY);
        }

        // Convertit la grille du backend dans le référentielle de la grille réel
        public Point gridToReal(Point grid_pos)
        {
            int offsetX = (REAL_GAME_WIDTH - GAME_WIDTH * BLOCK_SIZE) / 2;
            int offsetY = (REAL_GAME_HEIGHT - GAME_HEIGHT * BLOCK_SIZE) / 2;

            int realX = offsetX + grid_pos.X * BLOCK_SIZE + BLOCK_SIZE / 2;
            int realY = offsetY + grid_pos.Y * BLOCK_SIZE + BLOCK_SIZE / 2;

            return new Point(realX, realY);
        }
    }
}
