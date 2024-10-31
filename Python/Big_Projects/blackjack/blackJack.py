from random import randint
from time import sleep
class Deck():
    def __init__(self):
        listOfNumCards = [2,3,4,5,6,7,8,9,'J','Q','K','A']
        self.cards = {"blackLeaf" : listOfNumCards, "blackClover" : listOfNumCards, 'redHeart' : listOfNumCards, 'redDiamond' : listOfNumCards}

    def getCard(self,string,number):
        retValue = self.cards[string]
        print(retValue[number], string)

print("write your name please")
playerName = input()
player = Deck()
computer = Deck()
currentBudgetOfThePlayer = 1000

print('hello', playerName, ', this is a blackjack game, you start with 1000 dollars.\n')

def game(player, computer, currentBudgetOfThePlayer):


    isDealNumberValid = False
    while not isDealNumberValid:
        print('type a number you want to deal:')
        dealNumber = input()
        try:
            dealNumber = int(dealNumber)
            isDealNumberValid = True
        except:
            pass



    while (currentBudgetOfThePlayer - dealNumber) < 0 and currentBudgetOfThePlayer > 0:
                print("you cant type this num! try again")
                dealNumber = input()
                dealNumber = int(dealNumber)



    def addCard(player):
        typeCard = randint(0,3)
        numCard = randint(0,11)

        if typeCard == 1:
            player.getCard("blackLeaf", numCard)
        elif typeCard == 2:
            player.getCard("blackClover", numCard)
        elif typeCard == 2:
            player.getCard("redHeart", numCard)
        else:
            player.getCard("redDiamond", numCard)

        if numCard == 0:
            sumOfPlayerNum = 2
        elif numCard == 1:
            sumOfPlayerNum = 3
        elif numCard == 2:
            sumOfPlayerNum = 4
        elif numCard == 3:
            sumOfPlayerNum = 5
        elif numCard == 4:
            sumOfPlayerNum = 6
        elif numCard == 5:
            sumOfPlayerNum = 7
        elif numCard == 6:
            sumOfPlayerNum = 8
        elif numCard == 7:
            sumOfPlayerNum = 9
        elif numCard == 11:
            sumOfPlayerNum = 11
        else:
            sumOfPlayerNum = 10
        return sumOfPlayerNum

    currentBudgetOfThePlayer = currentBudgetOfThePlayer - dealNumber
    print('you have now :', currentBudgetOfThePlayer , 'dollars, lets start to play!')
    print("your cards are:")
    sumOfPlayerNum = addCard(player)
    sleep(1)
    print("and:")
    sumOfPlayerNum = sumOfPlayerNum + addCard(player)
    print('sum of your cards are:', sumOfPlayerNum)

    sleep(3)

    print('--------------------------')
    print("computer cards are:")
    sumOfComputerNum = addCard(computer)
    sleep(1)
    print("and:")
    sumOfComputerNum = sumOfComputerNum + addCard(computer)
    print('sum of computer cards are:', sumOfComputerNum)

    sleep(2)

    print('you can now select between stand or hit - stand for check the results,\n')
    print('and hit to pick up another card. WARNING - if the sum of your cards will be more than 21, the turn is automaticlly move to the computer.')
    playerOptionHitOrBust = input().upper()

    while playerOptionHitOrBust != 'STAND' and playerOptionHitOrBust != 'HIT':
        print('choose between hit and stand please.')
        playerOptionHitOrBust = input().upper()

    sleep(2)

    while playerOptionHitOrBust != 'STAND':
        sumOfPlayerNum = sumOfPlayerNum + addCard(player)
        print('sum of your cards are:', sumOfPlayerNum)
        if sumOfPlayerNum > 21:
            if sumOfComputerNum < 21 or sumOfComputerNum == 21:
                print('COMPUTER WINS! your budget is :', currentBudgetOfThePlayer)
                sleep(1)
                if currentBudgetOfThePlayer == 0:
                    print("that's over! thank you for playing!")
                    return
                else:
                    print('would you like to play another game?')
                    yesOrNo = input().upper()
                    while yesOrNo != 'YES' and yesOrNo != 'NO':
                        print('choose between yes and no please.')
                        yesOrNo = input().upper()
                    if yesOrNo == 'YES':
                        game(player, computer, currentBudgetOfThePlayer)
                    else:
                        return 'thanks you for playing.'
        sleep(1)

        if sumOfPlayerNum == 21:
            while sumOfComputerNum < 21:
                sumOfComputerNum = sumOfComputerNum + addCard(computer)
                print('sum of computer cards are:', sumOfComputerNum)
                sleep(1)
            if sumOfComputerNum == 21:
                currentBudgetOfThePlayer = currentBudgetOfThePlayer + dealNumber
                print('TIE! NO ONE WINS! you recived the amount you dealed with no profit. would you like to play another game?')
                yesOrNo = input().upper()
                while yesOrNo != 'YES' and yesOrNo != 'NO':
                    print('choose between yes and no please.')
                    yesOrNo = input().upper()
                if yesOrNo == 'YES':
                    game(player, computer, currentBudgetOfThePlayer)
                else:
                    return 'thanks you for playing.'
            else:
                currentBudgetOfThePlayer = currentBudgetOfThePlayer + (dealNumber*2.5)
                print('BLACK JACK! you WON! your amount now is: ', currentBudgetOfThePlayer)
                sleep(1)
                print('would you like to play another game?')
                yesOrNo = input().upper()
                while yesOrNo != 'YES' and yesOrNo != 'NO':
                    print('choose between yes and no please.')
                    yesOrNo = input().upper()
                if yesOrNo == 'YES':
                    game(player, computer, currentBudgetOfThePlayer)
                else:
                    return 'thanks you for playing.'
        sleep(1)

        if sumOfPlayerNum < 21:
            print('choose between hit and stand please.')
            playerOptionHitOrBust = input().upper()
            while playerOptionHitOrBust != 'STAND' and playerOptionHitOrBust != 'HIT':
                print('choose between hit and stand please.')
                playerOptionHitOrBust = input().upper()


    sleep(2)

    while sumOfComputerNum < 21 and ((sumOfComputerNum < sumOfPlayerNum) or (sumOfComputerNum == sumOfPlayerNum)):
        if sumOfComputerNum > sumOfPlayerNum:
            print('COMPUTER WINS! your budget is :', currentBudgetOfThePlayer)
            sleep(1)
            if currentBudgetOfThePlayer == 0:
                print("that's over! thank you for playing!")
                return
            else:
                print('would you like to play another game?')
                yesOrNo = input().upper()
                while yesOrNo != 'YES' and yesOrNo != 'NO':
                    print('choose between yes and no please.')
                    yesOrNo = input().upper()
                if yesOrNo == 'YES':
                    game(player, computer, currentBudgetOfThePlayer)
                else:
                    return 'thanks you for playing.'
        else:
            sumOfComputerNum = sumOfComputerNum + addCard(computer)
            print('sum of computer cards are:', sumOfComputerNum)
        sleep(1)
        if sumOfComputerNum > 21:
            currentBudgetOfThePlayer = currentBudgetOfThePlayer + (dealNumber*2)
            print('you WON! your amount now is: ', currentBudgetOfThePlayer)
            sleep(1)
            print('would you like to play another game?')
            yesOrNo = input().upper()
            while yesOrNo != 'YES' and yesOrNo != 'NO':
                print('choose between yes and no please.')
                yesOrNo = input().upper()
            if yesOrNo == 'YES':
                game(player, computer, currentBudgetOfThePlayer)
            else:
                return 'thanks you for playing.'
        sleep(1)
        if sumOfComputerNum == 21:
            print('COMPUTER WINS! your budget is :', currentBudgetOfThePlayer)
            sleep(1)
            if currentBudgetOfThePlayer == 0:
                print("that's over! thank you for playing!")
                return
            else:
                print('would you like to play another game?')
                yesOrNo = input().upper()
                while yesOrNo != 'YES' and yesOrNo != 'NO':
                    print('choose between yes and no please.')
                    yesOrNo = input().upper()
                if yesOrNo == 'YES':
                    game(player, computer, currentBudgetOfThePlayer)
                else:
                    return 'thanks you for playing.'
        sleep(1)
        if sumOfComputerNum < 21 and sumOfComputerNum > sumOfPlayerNum:
            print('COMPUTER WINS! your budget is :', currentBudgetOfThePlayer)
            sleep(1)
            if currentBudgetOfThePlayer == 0:
                print("that's over! thank you for playing!")
                return
            else:
                print('would you like to play another game?')
                yesOrNo = input().upper()
                while yesOrNo != 'YES' and yesOrNo != 'NO':
                    print('choose between yes and no please.')
                    yesOrNo = input().upper()
                if yesOrNo == 'YES':
                    game(player, computer, currentBudgetOfThePlayer)
                else:
                    return 'thank you for playing.'
        if sumOfComputerNum < 21 and sumOfComputerNum < sumOfPlayerNum:
            sleep(1)
            continue
game(player, computer, currentBudgetOfThePlayer)

