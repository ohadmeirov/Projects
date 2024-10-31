from random import randint
i = 1
rand_num = randint(1,100)

print('guess a number')
user_num = input()
user_num = int(user_num)

while user_num != rand_num and user_num != -1:
    if user_num > rand_num:
        print('try again! your num is bigger')
    else:
        print('try again! your num is lower')
    user_num = input()
    user_num = int(user_num)
    i = i + 1

if user_num == -1:
    print('why you gave up?')
else:
    if i == 1:
        print(f'on your {i}st time!')
    elif i == 2:
        print(f'on your {i}nd time!')
    elif i == 3:
        print(f'on your {i}rd time!')
    else:
        print(f'on your {i}th time!')