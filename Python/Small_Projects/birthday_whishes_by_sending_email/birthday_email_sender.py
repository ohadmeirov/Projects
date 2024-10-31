import smtplib
import datetime as dt
import random
import pandas

my_email = "ohadmeirov1606@gmail.com"
my_password = "ddmfznsbvdkynobw"

today = dt.datetime.now()
today_tuple = (today.month, today.day)

data = pandas.read_csv("one_hundred_days_of_code_projects/birthday_whishes_by_sending_email/birthdays.csv")
birthday_dict = {(data_row['month'], data_row['day']): data_row for (index, data_row) in data.iterrows()}

if today_tuple in birthday_dict:
    birthday_person = birthday_dict[today_tuple]
    file_path = f'one_hundred_days_of_code_projects/birthday_whishes_by_sending_email/letter_{random.randint(1, 3)}.txt'
    with open(file_path) as letter_file:
        contents = letter_file.read()
        contents = contents.replace("[NAME]", birthday_person['name'])

    with smtplib.SMTP("smtp.gmail.com") as connection:
        connection.starttls()
        connection.login(my_email, my_password)
        connection.sendmail(from_addr= my_email,
                            to_addrs= birthday_person["email"],
                            msg="Subject:Happy Birthday!\n\n{contents}"
                            )