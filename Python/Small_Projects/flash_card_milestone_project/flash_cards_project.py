from tkinter import *
import json
import pandas
import random

BACKGROUND_COLOR = "#B1DDC6"
current_card ={}
to_learn = {}

try:
    data = pandas.read_csv('one_hundred_days_of_code_projects/flash_card_milestone_project/words_to_learn.csv')
except FileNotFoundError:
    original_data = pandas.read_csv('one_hundred_days_of_code_projects/flash_card_milestone_project/french_words.csv')
    to_learn = original_data.to_dict(orient= "records")
else:
    to_learn = data.to_dict(orient= 'records')




def next_card():
    global current_card, flip_timer
    window.after_cancel(flip_timer)
    current_card= random.choice(to_learn)
    canvas.itemconfig(card_title, text='French', fill= 'black')
    canvas.itemconfig(card_word, text=current_card['French'], fill= 'black')
    canvas.itemconfig(card_background, image= flash_card_image_front)
    flip_timer = window.after(3000, func= flip_card)


def flip_card():
    canvas.itemconfig(card_title, text= "English", fill= 'white')
    canvas.itemconfig(card_word, text= current_card['English'], fill= 'white')
    canvas.itemconfig(card_background, image= flash_card_image_back)


def is_known():
    to_learn.remove(current_card)
    words_to_learn = pandas.DataFrame(to_learn)
    words_to_learn.to_csv('one_hundred_days_of_code_projects/flash_card_milestone_project/words_to_learn.csv', index= False)


    next_card()

window = Tk()
window.title("Flashy")
window.config(padx= 50, pady= 50, bg= BACKGROUND_COLOR)

flip_timer = window.after(3000, func= flip_card)

canvas = Canvas(width= 800, height= 526)

flash_card_image_front = PhotoImage(file="one_hundred_days_of_code_projects/flash_card_milestone_project/card_front.png")
flash_card_image_back = PhotoImage(file="one_hundred_days_of_code_projects/flash_card_milestone_project/card_back.png")

card_background = canvas.create_image(400, 263, image= flash_card_image_front)
canvas.config(bg= BACKGROUND_COLOR, highlightthickness= 0)
canvas.grid(column=0, row=0, columnspan= 2)

card_title = canvas.create_text(400, 150, text='', font= ('Ariel', 40, "italic"))
card_word = canvas.create_text(400, 263, text='', font= ('Ariel', 60, "bold"))


yes_image = PhotoImage(file='one_hundred_days_of_code_projects/flash_card_milestone_project/right.png')
known_button = Button(image= yes_image, highlightthickness= 0, command= is_known)
known_button.grid(column=1 ,row=1)

no_image = PhotoImage(file='one_hundred_days_of_code_projects/flash_card_milestone_project/wrong.png')
unknown_button = Button(image= no_image, highlightthickness= 0, command= next_card)
unknown_button.grid(column=0 ,row=1)


next_card()


window.mainloop()