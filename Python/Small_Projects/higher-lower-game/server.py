from flask import Flask
import random

app = Flask(__name__)

@app.route('/')
def creating_game():
    return '<h1 style= "color:red">Guess a number between 1 to 9</h1>' \
            '<img src=https://media.giphy.com/media/3o7aCSPqXE5C6T8tBC/giphy.gif>'

user_guess = random.randint(1,10)

@app.route('/<int:rand_num>')
def guessed_number(rand_num):
    if user_guess == rand_num:
        return '<h1 style= "color:blue", "text-align:center"><b><em>You Right!</em></b></h1>'
    elif user_guess < rand_num:
        return '<h3>You Wrong! You chose lower number, TRY AGAIN</h3>'
    else:
        return '<h3>You Wrong! You chose higher number, TRY AGAIN</h3>'




if __name__ == '__main__':
    app.run(debug=True)