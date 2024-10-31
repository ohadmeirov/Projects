import requests
from bs4 import BeautifulSoup
import re
from selenium import webdriver
from selenium.webdriver.common.by import By
import time

GOOGLE_FORMS_LINK = "https://docs.google.com/forms/d/e/1FAIpQLSfZNVt18xrEltEw3oQ5yd4NgBzMWAjGlDl3qI1Eniw0DQBGOQ/viewform?usp=sf_link"
ZILLOW_CLONE_WEBSITE = "https://appbrewery.github.io/Zillow-Clone/"

header = {
    "User-Agent": 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
    "Accept-Language": 'he-IL,he;q=0.9,en-US;q=0.8,en;q=0.7',
}

response = requests.get(ZILLOW_CLONE_WEBSITE, headers= header)
soup = BeautifulSoup(response.text, 'html.parser')

li_elements = soup.find_all('li')
href_list_links = []
price_list = []
address_list  = []


li_elements = soup.find_all('li')

for li in li_elements:
    a_element = li.find('a')

    if a_element:
        href_value = a_element.get('href')
        href_list_links.append(href_value)



for li in li_elements:

    target_a = li.find('a')

    if target_a:
        href_value = target_a.get('href')
        href_list_links.append(href_value)


    target_span = li.find('span', class_='PropertyCardWrapper__StyledPriceLine')

    if target_span:
        price_value = target_span.get_text(strip=True)
        price_list.append(price_value)


    target_adress = li.find('div', class_='StyledPropertyCardDataWrapper')

    if target_adress:
        adress_value = target_adress.get_text(strip=True)
        address_list.append(adress_value)


print(href_list_links)

pattern = re.compile(r'\$\d[\d,]*')
new_prices_list = pattern.findall(' '.join(price_list))
print(new_prices_list)


pattern = re.compile(r'(.*)(\$\d[\d,]+[/+]*mo\d+b[ds]\d+ba\d+sqft- Apartment for rent)')

cleaned_addresses = []
for address in address_list:
    match = pattern.match(address)
    if match:
        cleaned_address = match.group(1).strip()

        cleaned_address = re.sub(r'[\n|]+', ' ', cleaned_address)

        cleaned_address = re.sub(r'\b\d{5}\b', '', cleaned_address)

        cleaned_address = re.sub(r'#\d{3}', '', cleaned_address)

        cleaned_address = re.sub(r'#.{6}', '', cleaned_address)

        cleaned_addresses.append(cleaned_address)


print(cleaned_addresses)








chrome_options = webdriver.ChromeOptions()
chrome_options.add_experimental_option("detach", True)
driver = webdriver.Chrome(options=chrome_options)

for n in range(len(href_list_links)):
    driver.get(GOOGLE_FORMS_LINK)
    time.sleep(2)

    address = driver.find_element(by=By.XPATH,
    value='//*[@id="mG61Hd"]/div[2]/div/div[2]/div[1]/div/div/div[2]/div/div[1]/div/div[1]/input')

    price = driver.find_element(by=By.XPATH,
    value='//*[@id="mG61Hd"]/div[2]/div/div[2]/div[2]/div/div/div[2]/div/div[1]/div/div[1]/input')

    link = driver.find_element(by=By.XPATH,
    value='//*[@id="mG61Hd"]/div[2]/div/div[2]/div[3]/div/div/div[2]/div/div[1]/div/div[1]/input')

    submit_button = driver.find_element(by=By.XPATH,
    value='//*[@id="mG61Hd"]/div[2]/div/div[3]/div[1]/div[1]/div')

    address.send_keys(address_list[n])
    price.send_keys(new_prices_list[n])
    link.send_keys(href_list_links[n])
    submit_button.click()