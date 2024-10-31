from turtle import Turtle, Screen
import time
from snake_class import Snake
from food_class import Food
from scoreboard_class import Scroeboard


screen = Screen()
screen.setup(width = 600, height = 600)
screen.bgcolor("black")
screen.title("Snake Game")
screen.tracer(0)

snake = Snake()
food = Food()
scroeboard = Scroeboard()

screen.listen()
screen.onkey(snake.up, "Up")
screen.onkey(snake.down, "Down")
screen.onkey(snake.left, "Left")
screen.onkey(snake.right, "Right")

game_is_on = True
while game_is_on:
    screen.update()
    time.sleep(0.08)
    snake.move()

    if snake.head.distance(food) < 15:
        food.refresh()
        snake.extend()
        scroeboard.increase_score()


    if snake.head.xcor() > 280 or snake.head.xcor() < -280 or snake.head.ycor() > 280 or snake.head.ycor() < -280:
        scroeboard.resetGame()
        snake.resetSnake()

    for segmant in snake.players[1:]:
        if snake.head.distance(segmant) < 10:
            game_is_on = False
            scroeboard.resetGame()
            snake.resetSnake()








screen.exitonclick()