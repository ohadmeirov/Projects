from turtle import Turtle

FONT = ("Courier", 16, "normal")

class ScoreBoard(Turtle):

    def __init__(self):
        super().__init__()
        self.score = 1
        self.hideturtle()
        self.penup()
        self.update_scoreboard()

    def update_scoreboard(self):
        self.clear()
        self.goto(-240, 270)
        self.write(f"Level: {self.score}", align="center", font= FONT)


    def update_score(self):
        self.score += 1
        self.update_scoreboard()

    def game_over(self):
        self.goto(0, 0)
        self.write("GAME OVER", align="center", font= FONT)