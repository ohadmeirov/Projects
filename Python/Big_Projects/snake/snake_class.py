from turtle import Turtle, Screen

STARTING_POSITION = [(0, 0), (-20, 0), (-40, 0)]
MOVE_DISTANCE = 20
UP = 90
DOWN = 270
LEFT = 180
RIGHT = 0

class Snake:

    def __init__(self):
        self.players = []
        self.crate_snake()
        self.head = self.players[0]

    def crate_snake(self):
        for position in STARTING_POSITION:
            game_turtle = Turtle("square")
            game_turtle.color("white")
            game_turtle.penup()
            game_turtle.goto(position)
            self.players.append(game_turtle)


    def move(self):
        for segment_number in range(len(self.players) - 1, 0, -1):
            new_x = self.players[segment_number - 1].xcor()
            new_y = self.players[segment_number - 1].ycor()
            self.players[segment_number].goto(new_x, new_y)
        self.head.forward(MOVE_DISTANCE)

    def extend(self):
        self.add_segment(self.players[-1].position())

    def resetSnake(self):
        for seg in self.players:
            seg.goto(1000, 1000)
        self.players.clear()
        self.crate_snake()
        self.head = self.players[0]

    def add_segment(self, position):
        game_turtle = Turtle("square")
        game_turtle.color("white")
        game_turtle.penup()
        game_turtle.goto(position)
        self.players.append(game_turtle)


    def up(self):
        if self.head.heading() != DOWN:
            self.head.setheading(UP)

    def down(self):
        if self.head.heading() != UP:
            self.head.setheading(DOWN)

    def left(self):
        if self.head.heading() != RIGHT:
            self.head.setheading(LEFT)

    def right(self):
        if self.head.heading() != LEFT:
            self.head.setheading(RIGHT)