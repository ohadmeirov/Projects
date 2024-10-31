from turtle import Turtle
ALIGNMENT = 'center'
FONT = ("Arial", 20, "normal")
class Scroeboard(Turtle):

    def __init__(self):
        super().__init__()
        self.score = 0
        with open("C:\python_projects_ohad\py_one_hundred_days_of_code_projects\snake\data.txt", "r") as data:
            self.high_score = data.read()
        self.color("white")
        self.penup()
        self.goto(0,270)
        self.hideturtle()
        self.update_scoreboard()

    def resetGame(self):
        if self.score > self.high_score:
            self.high_score = self.score
            with open("C:\python_projects_ohad\py_one_hundred_days_of_code_projects\snake\data.txt", "w") as data:
                data.write(f'{self.high_score}')
        self.score = 0
        self.clear()
        self.update_scoreboard()


    def update_scoreboard(self):
        self.write(f'Score: {self.score} High Score: {self.high_score}', align= ALIGNMENT, font= FONT)

    def increase_score(self):
        self.score += 1
        self.clear()
        self.update_scoreboard()