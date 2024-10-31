from tkinter import *
from tkinter import messagebox
from random import choice, randint, shuffle
import pyperclip
import json
# ---------------------------- PASSWORD GENERATOR ------------------------------- #
def generate_password():
    letters = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']
    numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
    symbols = ['!', '#', '$', '%', '&', '(', ')', '*', '+']

    nr_letter = randint(8, 10)
    nr_symbols = randint(2, 4)
    nr_numbers = randint(2, 4)

    password_letters = [choice(letters) for _ in range(nr_letter)]
    password_symbols = [choice(symbols) for _ in range(nr_symbols)]
    password_numbers = [choice(numbers) for _ in range(nr_numbers)]

    password_list = password_letters + password_symbols + password_numbers

    shuffle(password_list)

    password = "".join(password_list)
    password_entry.insert(0, password)
    pyperclip.copy(password)

# ---------------------------- SAVE PASSWORD ------------------------------- #

def save():
    website = website_entry.get()
    email = email_entry.get()
    password = password_entry.get()
    new_data = {
        website: {
            "email": email,
            "password": password,
        }
    }
    if len(website) == 0 or len(password) == 0:
        messagebox.showinfo(title='Oops', message="Please make sure you haven't left any fields empty")
    else:
        try:
            with open('C:\python_projects_ohad\one_hundred_days_of_code_projects\password_manager\data.json', "r") as data_file:
                data = json.load(data_file)

        except FileNotFoundError:

            json.dump(new_data, data_file, indent= 4)
        else:

            data.update(new_data)
            with open('C:\python_projects_ohad\one_hundred_days_of_code_projects\password_manager\data.json', "w") as data_file:
                json.dump(data, data_file, indent= 4)
        finally:

            website_entry.delete(0, END)
            password_entry.delete(0, END)




def find_password():
    website = website_entry.get()
    try:
        with open('C:\python_projects_ohad\one_hundred_days_of_code_projects\password_manager\data.json', "r") as data_file:
            data = json.load(data_file)

    except FileNotFoundError:
        messagebox.showinfo(title= "Error", message= "No Data File Found.")

    else:
        if website in data:
            email = data[website]['email']
            password = data[website]['password']
            messagebox.showinfo(title= website, message= f'Emal: {email}\nPassword: {password}')
        else:
            messagebox.showinfo(title= website, message= f'No details for {website} exists.')

# ---------------------------- UI SETUP ------------------------------- #

window = Tk()
window.title("Password Manager")
window.config(padx= 50, pady= 50)



website_label = Label(text= 'Website:')
email_label = Label(text= 'Email/Username:')
password_label = Label(text= 'Password:')

website_label.grid(column=0, row=1)
email_label.grid(column=0, row=2)
password_label.grid(column=0, row=3)


website_entry = Entry(width= 24)
email_entry = Entry(width= 42)
password_entry = Entry(width= 24)

website_entry.grid(column=1, row=1, columnspan=1)
email_entry.grid(column=1, row=2, columnspan=2)
password_entry.grid(column=1, row=3)


generate_password_button = Button(text='Generate Password', command= generate_password)
add_button = Button(text='Add', width=36, command=save)
search_button = Button(text="Search", command= find_password)

generate_password_button.grid(column=2, row=3)
add_button.grid(column=1, row=4, columnspan=2)
search_button.grid(column=2, row=1)

website_entry.focus()
email_entry.insert(0, "@gmail.com")

canvas = Canvas(height= 200, width= 200)
logo_img = PhotoImage(file= "C:\python_projects_ohad\one_hundred_days_of_code_projects\password_manager\logo.png")
canvas.create_image(100, 100, image= logo_img)
canvas.grid(column=1, row= 0)





window.mainloop()