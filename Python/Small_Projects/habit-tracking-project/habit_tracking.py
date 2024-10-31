import requests
from datetime import datetime

USERNAME = 'ohad'
TOKEN = 'blablabla5'
GRAPH_ID = 'graph1'

today = datetime.now()
TIME_TODAY = today.strftime('%Y%m%d')
TIME_YESTERDAY = '20231227'

pixela_endpoint = "https://pixe.la/v1/users"

user_params = {
    'token': TOKEN,
    'username': USERNAME,
    'agreeTermsOfService': 'yes',
    'notMinor': 'yes',
}


graph_endpoint = f'{pixela_endpoint}/{USERNAME}/graphs'

graph_config = {
    'id': GRAPH_ID,
    'name': 'Cycling Graph',
    'unit': 'Km',
    'type': 'float',
    'color': 'ajisai',
}

headers = {
    'X-USER-TOKEN': TOKEN
}

pixel_creation_endpoint = f'{pixela_endpoint}/{USERNAME}/graphs/{GRAPH_ID}'

pixel_data = {
    'date': TIME_TODAY,
    'quantity': '9.74',
}

update_pixel_endpoint = f'{pixela_endpoint}/{USERNAME}/graphs/{GRAPH_ID}/{TIME_TODAY}'

new_pixel_data = {
    'quantity': '4.5',
}

delete_pixel_endpoint = f'{pixela_endpoint}/{USERNAME}/graphs/{GRAPH_ID}/{TIME_TODAY}'


response = requests.delete(url= delete_pixel_endpoint, headers= headers)
print(response.text)
