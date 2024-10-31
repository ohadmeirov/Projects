from turtle import Turtle, Screen
import time
from player import Player
from car_manager import CarManager
from scoreboard_turtle import ScoreBoard

screen = Screen()
screen.setup(width = 600, height = 600)
screen.tracer(0)

player = Player()
scoreBoard = ScoreBoard()
car_object = CarManager()


screen.listen()
screen.onkey(player.go_up, "Up")

game_is_on = True
while game_is_on:
    time.sleep(0.1)
    screen.update()

    car_object.create_car()
    car_object.move_backward()

    for car in car_object.all_cars:
        if car.distance(player) < 20:
            scoreBoard.game_over()
            game_is_on = False


    if player.win_check():
        player.reset_or_start_game()
        car_object.level_up()
        scoreBoard.update_score()




screen.exitonclick()