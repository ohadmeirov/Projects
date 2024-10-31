import requests
from bs4 import BeautifulSoup
import lxml
import smtplib

url = 'https://www.amazon.com/dp/B075CYMYK6?psc=1&ref_=cm_sw_r_cp_ud_ct_FM9M699VKHTT47YD50Q6'

my_email= "ohadmeirov1606@gmail.com"
my_password = "ddmfznsbvdkynobw"

req_header = {
    "User-Agent": 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
    "Accept-Language": 'he-IL,he;q=0.9,en-US;q=0.8,en;q=0.7',
}

response = requests.get(url= url, headers= req_header)

soup = BeautifulSoup(response.content, 'lxml')

price = soup.find(class_="a-offscreen").get_text()
price_without_currency = price.split("$")[1]
price_as_float = float(price_without_currency)



title = soup.find(id="productTitle").get_text().strip()

BUY_PRICE = 80

if price_as_float < BUY_PRICE:
    message = f"{title} is now {price}"

    with smtplib.SMTP("smtp.gmail.com", port=587) as connection:
        connection.starttls()
        result = connection.login(my_email, my_password)
        connection.sendmail(
            from_addr=my_email,
            to_addrs=my_email,
            msg=f"Subject:Amazon Price Alert!\n\n{message}\n{url}".encode("utf-8")
        )

