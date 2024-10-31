arr = [' '] * 9



#build the table
def tableDrawer(arr):
    print(arr[0],   '|'    , arr[1]    ,'|' , arr[2])
    print("---------")
    print(arr[3],   '|'    , arr[4]    ,'|' , arr[5])
    print("---------")
    print(arr[6],   '|'    , arr[7]    ,'|' , arr[8])

#built the table, now have to get the inputs from the users and act accordingly.

print("first user, please type X or O")
firstUserInput = input().upper()

#check that everything alright
while firstUserInput != 'X' and firstUserInput != 'O':
    print("please type X or O.\ntype again now:")
    firstUserInput = input().upper()

if firstUserInput == 'X':
    secondUserInput = 'O'
else:
    secondUserInput = 'X'




#now have to check the win

def checkWin(arr,numberOnTheTableFromTheUser):
    if numberOnTheTableFromTheUser == 0:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 6]) or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 4] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 8]):
            return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 2:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 6]) or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 2] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 4]):
            return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 6:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 6]) or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 2] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 4]):
            return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 8:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 6]) or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 4] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 8]):
            return numberOnTheTableFromTheUser

    elif numberOnTheTableFromTheUser == 1:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 6]):
          return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 3:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3]):
          return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 5:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3]):
          return numberOnTheTableFromTheUser
    elif numberOnTheTableFromTheUser == 1:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 6]):
          return numberOnTheTableFromTheUser

    elif numberOnTheTableFromTheUser == 4:
        if (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 1] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 1])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 3] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser -3]) or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 2] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser -2])or (arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser + 4] and arr[numberOnTheTableFromTheUser] == arr[numberOnTheTableFromTheUser - 4]):
          return numberOnTheTableFromTheUser

    return -1




#got the users input, now have to play the game by ask for a number and then place it in the table in the same number

def game(firstUserInput, secondUserInput, arr):
    timeOfPlayingTheGame_AndCounterToKnowWhichMarkHasToBe = 0

    #get the number from the user
    while timeOfPlayingTheGame_AndCounterToKnowWhichMarkHasToBe !=9:
        print("type a number between 0-8 which you want to place your mark (X or O) on the table.\nlowest row is : 6 7 8 , mid row is: 4 5 6 and the highest row is: 0 1 2.")
        numberOnTheTableFromTheUser = input()
        numberOnTheTableFromTheUser = int(numberOnTheTableFromTheUser)

        #check that everything alright
        while numberOnTheTableFromTheUser < 0 or numberOnTheTableFromTheUser>8:
            print("please type a number between 0-8.\ntype again now:")
            numberOnTheTableFromTheUser = input()
            numberOnTheTableFromTheUser = int(numberOnTheTableFromTheUser)

        #check that the typed number isnt already typed and between 0-8
        while (arr[numberOnTheTableFromTheUser] == 'X' or arr[numberOnTheTableFromTheUser] == 'O') or (numberOnTheTableFromTheUser < 0 or numberOnTheTableFromTheUser>8):
            print("please type another number between 0-8.\nthe number you typed is already typed or not between 0-8\ntype again now:")
            numberOnTheTableFromTheUser = input()
            numberOnTheTableFromTheUser = int(numberOnTheTableFromTheUser)

        #place the right mark on the table
        if timeOfPlayingTheGame_AndCounterToKnowWhichMarkHasToBe % 2 == 0:
            arr[numberOnTheTableFromTheUser] = firstUserInput
        else:
            arr[numberOnTheTableFromTheUser] = secondUserInput
        winNum = checkWin(arr,numberOnTheTableFromTheUser)
        if winNum != -1:
            print(arr[winNum], 'is the winner!')
            break
        timeOfPlayingTheGame_AndCounterToKnowWhichMarkHasToBe = timeOfPlayingTheGame_AndCounterToKnowWhichMarkHasToBe + 1
        tableDrawer(arr)
    if winNum == -1:
        print('DRAW !')


game(firstUserInput, secondUserInput, arr)