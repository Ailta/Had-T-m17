using System.Diagnostics;
using System.Xml.Linq;

namespace Snake
{
    public partial class Snake
    {
        // povolene smery pohybu hada
        public enum Direction
        {
            Right,
            Left,
            Up,
            Down,
        }

        // nedostane data hry, vraci jmeno hada
        public string? Index()
        {
            return Name;
        }

        // dostane data hry, vraci prazdnou odpoved
        public string Init(Game game)
        {
            return string.Empty;
        }

        private void DebugWrite(Game game, int iteration, string text)
        {
            if (iteration == 0)
                Debug.WriteLine(text);
            else if (game.Iteration == iteration)
                Debug.WriteLine(text);
        }

        public bool CheckIfBlockedFor(Game game, Coordinate pos, Coordinate[] possibleObstacle)
        {
            bool isBlocked = false;

            for (int obstacleI = 0; obstacleI < possibleObstacle.Length; obstacleI++)
            {
                var obstacle = possibleObstacle[obstacleI];

                if (obstacle.X == pos.X || obstacle.Y == pos.Y)
                    isBlocked = true;
            }

            return isBlocked;
        }

        public Tuple<bool, bool, bool> CheckIfBlocked(Game game, Coordinate pos)
        {
            bool isBlocked = false;
            bool isBlockedByEnemyHead = false;
            bool outsideOfBoundary = false;

            // outside of board ?
            if (game.Board.Width == pos.X || game.Board.Height == pos.Y)
                isBlocked = true;
            if (-1 == pos.Y || -1 == pos.X)
                isBlocked = true;

            // in obstacle ?
            isBlocked = CheckIfBlockedFor(game, pos, game.Board.Obstacles);

            // in our body ?
            isBlocked = CheckIfBlockedFor(game, pos, game.You.Body);

            // for enemy
            for (int enemySnakeI = 0; enemySnakeI < game.Board.Snakes.Length; enemySnakeI++)
            {
                var enemySnake = game.Board.Snakes[enemySnakeI];

                // in enemy body ?
                isBlocked = CheckIfBlockedFor(game, pos, enemySnake.Body);

                // in enemy head ?
                if (enemySnake.Head.X == pos.X || enemySnake.Head.Y == pos.Y)
                    isBlockedByEnemyHead = true;
            }

            return new Tuple<bool, bool, bool>(isBlocked, isBlockedByEnemyHead, outsideOfBoundary);
        }

        // dostane data hry, vraci smer pohybu hada
        public Direction Move(Game game)
        {
            Direction direction;
            direction = Direction.Left;

            var snake = game.You;

            int amountOfFields = game.Board.Height * game.Board.Width;
            bool[] blockedFields = new bool[amountOfFields];

            Debug.WriteLine(amountOfFields);

            for (int checkedFieldI = 0; checkedFieldI < amountOfFields; checkedFieldI++)
            {
                Coordinate checkingFieldPos = new Coordinate()
                {
                    Y = checkedFieldI / game.Board.Width,
                    X = checkedFieldI - (game.Board.Width * (checkedFieldI / game.Board.Width))
                };

                DebugWrite(game, 1, $"{checkedFieldI}: {checkingFieldPos.X}, {checkingFieldPos.Y}");

                Tuple<bool, bool, bool> checkingField = CheckIfBlocked(game, new Coordinate() { X = checkingFieldPos.X, Y = checkingFieldPos.Y });
                Tuple<bool, bool, bool> checkingFieldLeft = CheckIfBlocked(game, new Coordinate() { X = checkingFieldPos.X - 1, Y = checkingFieldPos.Y });
                Tuple<bool, bool, bool> checkingFieldRight = CheckIfBlocked(game, new Coordinate() { X = checkingFieldPos.X + 1, Y = checkingFieldPos.Y });
                Tuple<bool, bool, bool> checkingFieldUp = CheckIfBlocked(game, new Coordinate() { X = checkingFieldPos.X, Y = checkingFieldPos.Y + 1 });
                Tuple<bool, bool, bool> checkingFieldDown = CheckIfBlocked(game, new Coordinate() { X = checkingFieldPos.X, Y = checkingFieldPos.Y - 1 });

                if (!checkingField.Item3)
                {
                    if (checkingFieldRight.Item1)
                        blockedFields[(checkingFieldPos.X - 1) + (checkingFieldPos.Y * 12)] = true;
                    if (checkingFieldRight.Item2)
                        blockedFields[(checkingFieldPos.X) + (checkingFieldPos.Y * 12)] = true;
                }

                if (!checkingFieldLeft.Item3)
                {
                    if (checkingFieldLeft.Item1)
                        blockedFields[(checkingFieldPos.X - 1) + (checkingFieldPos.Y * 12)] = true;
                    if (checkingFieldLeft.Item2)
                        blockedFields[(checkingFieldPos.X) + (checkingFieldPos.Y * 12)] = true;
                }

                if (!checkingFieldRight.Item3)
                {
                    if (checkingFieldRight.Item1)
                        blockedFields[(checkingFieldPos.X - 1) + (checkingFieldPos.Y * 12)] = true;
                    if (checkingFieldRight.Item2)
                        blockedFields[(checkingFieldPos.X) + (checkingFieldPos.Y * 12)] = true;
                }

                if (!checkingFieldUp.Item3)
                {
                    if (checkingFieldUp.Item1)
                        blockedFields[(checkingFieldPos.X - 1) + (checkingFieldPos.Y * 12)] = true;
                    if (checkingFieldUp.Item2)
                        blockedFields[(checkingFieldPos.X) + (checkingFieldPos.Y * 12)] = true;
                }

                if (!checkingFieldDown.Item3)
                {
                    if (checkingFieldDown.Item1)
                        blockedFields[(checkingFieldPos.X - 1) + (checkingFieldPos.Y * 12)] = true;
                    if (checkingFieldDown.Item2)
                        blockedFields[(checkingFieldPos.X) + (checkingFieldPos.Y * 12)] = true;
                }

            }

            /*if (snake.Head.X == closestFoodPos.X && snake.Head.Y == closestFoodPos.Y)
                closestFoodPos = null;*/


            return direction;
        }
    }
}
