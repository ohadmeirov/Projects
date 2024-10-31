from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
import time

chrome_options = webdriver.ChromeOptions()
chrome_options.add_experimental_option('detach', True)
CHROME_DRIVER_PATH = "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Google Chrome.lnk"
driver = webdriver.Chrome(options= chrome_options)

driver.get('https://www.yad2.co.il/personal/my-ads')

cookie = driver.find_element(By.XPATH, value= '//*[@id="__next"]/div/main/div/div/div/div[1]/div[5]/div/div[2]/button[3]')
cookie.click()


