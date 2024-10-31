PLACEHOLDER = '[name]'

with open("invited_names.txt", mode= "r") as names_in_the_file:
    names = names_in_the_file.readlines()

with open('letter.txt', mode= 'r') as letter_file:
    letter_contents = letter_file.read()
    for name in names:
        stripped_name = name.strip()
        new_letter = letter_contents.replace(PLACEHOLDER, stripped_name)
        with open(f'letter_to_{stripped_name}.txt', 'w') as final_letter:
            final_letter.write(new_letter)