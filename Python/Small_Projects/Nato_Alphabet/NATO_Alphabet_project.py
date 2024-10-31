import pandas

data = pandas.read_csv('nato_phonetic_alphabet.csv')

phonetic_dict = {row.letter:row.code for (index,row) in data.iterrows()}
print(phonetic_dict)

def generate_phonetic():
    word_from_user = input('Enter a word: ').upper()

    try:
        nato_with_user_input_list = [phonetic_dict[letter] for letter in word_from_user]
    except KeyError:
        print("Sorry, only letters in the alphabet please.")
        generate_phonetic()
    else:
        print(nato_with_user_input_list)

generate_phonetic()