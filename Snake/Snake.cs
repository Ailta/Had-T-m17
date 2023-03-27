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

        private Direction SnakeMovement(Game game, int X, int Y)
        {
            Direction direction = Direction.Up;
            var snake = game.You;

            bool toLeft = true;
            bool toRight = true;
            bool toUp = true;
            bool toDown = true;

            for (int bodyI = 0; bodyI < snake.Body.Length; bodyI++)
            {
                var body = snake.Body[bodyI];

                if (body.Y == snake.Head.Y)
                {
                    if (body.X == snake.Head.X - 1)
                        toLeft = false;
                    if (body.X == snake.Head.X + 1)
                        toRight = false;
                }

                if (body.X == snake.Head.X)
                {
                    if (body.Y == snake.Head.Y - 1)
                        toDown = false;
                    if (body.Y == snake.Head.Y + 1)
                        toUp = false;
                }
            }

            for (int enemySnakeI = 0; enemySnakeI < game.Board.Snakes.Length; enemySnakeI++)
            {
                var enemySnake = game.Board.Snakes[enemySnakeI];

                for (int enemyBodyI = 0; enemyBodyI < enemySnake.Body.Length; enemyBodyI++)
                {
                    var body = enemySnake.Body[enemyBodyI];

                    if (body.Y == snake.Head.Y)
                    {
                        if (body.X == snake.Head.X - 1)
                            toLeft = false;
                        if (body.X == snake.Head.X + 1)
                            toRight = false;
                    }

                    if (body.X == snake.Head.X)
                    {
                        if (body.Y == snake.Head.Y - 1)
                            toDown = false;
                        if (body.Y == snake.Head.Y + 1)
                            toUp = false;
                    }
                }

                if (enemySnake.Head.Y == snake.Head.Y)
                {
                    if (enemySnake.Head.X == snake.Head.X - 1)
                        toLeft = false;
                    if (enemySnake.Head.X == snake.Head.X + 1)
                        toRight = false;
                }

                if (enemySnake.Head.X == snake.Head.X)
                {
                    if (enemySnake.Head.Y == snake.Head.Y - 1)
                        toDown = false;
                    if (enemySnake.Head.Y == snake.Head.Y + 1)
                        toUp = false;
                }
            }

            for (int obstacleI = 0; obstacleI < game.Board.Obstacles.Length; obstacleI++)
            {
                var obstacle = game.Board.Obstacles[obstacleI];

                if (obstacle.Y == snake.Head.Y)
                {
                    if (obstacle.X == snake.Head.X - 1)
                        toLeft = false;
                    if (obstacle.X == snake.Head.X + 1)
                        toRight = false;
                }

                if (obstacle.X == snake.Head.X)
                {
                    if (obstacle.Y == snake.Head.Y - 1)
                        toDown = false;
                    if (obstacle.Y == snake.Head.Y + 1)
                        toUp = false;
                }
            }

            if (snake.Head.X - 1 < 0)
                toLeft = false;
            if (snake.Head.Y - 1 < 0)
                toDown = false;
            if (snake.Head.X + 1 >= game.Board.Width)
                toRight = false;
            if (snake.Head.Y + 1 >= game.Board.Height)
                toUp = false;

            Debug.WriteLine($"Left: {toLeft}; Right: {toRight}\nUp: {toUp}; Down: {toDown}");

            if (snake.Head.X > X && toLeft)
            {
                direction = Direction.Left;
                Debug.WriteLine("levá");
            }
            else if (snake.Head.X < X && toRight)
            {
                direction = Direction.Right;
                Debug.WriteLine("pravá");
            }
            else if (snake.Head.Y < Y && toUp)
            {
                direction = Direction.Up; 
                Debug.WriteLine("hore");
            }
            else if (snake.Head.Y > Y && toDown)
            {
                direction = Direction.Down;
                Debug.WriteLine("dole");
            }
            else
            {
                if (toRight)
                    direction = Direction.Right;
                if (toLeft)
                    direction = Direction.Left;
                if (toUp)
                    direction = Direction.Up;
                if (toDown)
                    direction = Direction.Down;
            }

            return direction;
        }

        // dostane data hry, vraci smer pohybu hada
        public Direction Move(Game game)
        {
            Direction direction;
            var snake = game.You;

            Coordinate closestFoodPos = new Coordinate();
            closestFoodPos.X = 0;
            closestFoodPos.Y = 0;

            for (int foodI = 0; foodI < game.Board.Food.Length; foodI++)
            {
                var food = game.Board.Food[foodI];

                int distanceXHeadToFood = snake.Head.X - food.X;
                int distanceYHeadToFood = snake.Head.Y - food.Y;

                int distanceXHeadToClosestFood = snake.Head.X - closestFoodPos.X;
                int distanceYHeadToClosestFood = snake.Head.Y - closestFoodPos.Y;

                double distanceHeadToFood = Math.Sqrt(distanceXHeadToFood * distanceXHeadToFood + distanceYHeadToFood * distanceYHeadToFood);
                double closestDistanceHeadToFood = Math.Sqrt(distanceXHeadToClosestFood * distanceXHeadToClosestFood + distanceYHeadToClosestFood * distanceYHeadToClosestFood);

                if (closestDistanceHeadToFood > distanceHeadToFood)
                {
                    closestFoodPos.X = food.X;
                    closestFoodPos.Y = food.Y;
                }
            }


            direction = SnakeMovement(game, closestFoodPos.X, closestFoodPos.Y);
            /*if (snake.Head.X == closestFoodPos.X && snake.Head.Y == closestFoodPos.Y)
                closestFoodPos = null;*/


            return direction;
        }
    }
}
