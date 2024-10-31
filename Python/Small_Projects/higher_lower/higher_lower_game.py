from data_for_higer_lower import data
import random

wins_counter = 0
flag = ''
player_one = random.randint(0,data.__len__()-1)
player_two = random.randint(0,data.__len__()-1)
while player_one == player_two:
    player_two = random.randint(0,data.__len__()-1)

print("Welcome to Higher or Lower game\n")

def game(wins_counter,player_one,player_two,flag):
    if wins_counter > 0:
        if flag == 'A is the winner':
            player_one = player_two
            player_two = random.randint(0,data.__len__()-1)
        else:
            player_two = random.randint(0,data.__len__()-1)
    one = data[player_one]
    two = data[player_two]

    print(f'you currently have {wins_counter} wins.\nthe options are:\n')
    print('A - ',one['name'], 'is a', one['description'], 'and from', one['country'])
    print('aginst')
    print('B - ',two['name'], 'is a', two['description'], 'and from', two['country'],'\n')

    user_input = input("which one from the followings has more followers? choose A or B: ").upper()

    while user_input != 'A' and user_input != 'B':
        user_input = input("please choose A or B: ").upper()

    if user_input == 'A':
        if one['follower_count'] > two['follower_count']:
            wins_counter += 1
            print('nice, you win. \n')
            flag = 'A is the winner'
            game(wins_counter,player_one,player_two,flag)
        else:
            print(f'you lost. you got {wins_counter} wins.')
            return
    else:
        if one['follower_count'] < two['follower_count']:
            wins_counter += 1
            print('nice, you win. \n')
            flag = 'B is the winner'
            game(wins_counter,player_one,player_two,flag)
        else:
            print(f'you lost. you got {wins_counter} wins.')
            return

game(wins_counter,player_one,player_two,flag)