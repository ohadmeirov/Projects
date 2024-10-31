from bs4 import BeautifulSoup
import requests
import spotipy
from spotipy.oauth2 import SpotifyOAuth

my_client_id = '10a8ad5c762b4ffcad76ac17bf7f3566'
my_client_secret = 'b803ca205fda485ea7f0a13d2d376f2d'

date_to_travel = input('Which year do you want to travel to? Type the date in this format, YYYY-MM-DD :\n')
url =  f'https://www.billboard.com/charts/hot-100/{date_to_travel}/'

response = requests.get(url)

soup = BeautifulSoup(response.text, 'html.parser')
song_names_spans = soup.select("li ul li h3")
song_names = [song.getText().strip() for song in song_names_spans]

sp = spotipy.Spotify(
    auth_manager=SpotifyOAuth(
        scope="playlist-modify-private",
        redirect_uri="http://example.com",
        client_id=my_client_id,
        client_secret=my_client_secret,
        show_dialog=True,
        cache_path="token.txt",
        username='Ohadmeirov',
    )
)
user_id = sp.current_user()["id"]


song_uris = []
year = date_to_travel.split("-")[0]
for song in song_names:
    result = sp.search(q=f"track:{song} year:{year}", type="track")
    print(result)
    try:
        uri = result["tracks"]["items"][0]["uri"]
        song_uris.append(uri)
    except IndexError:
        print(f"{song} doesn't exist in Spotify. Skipped.")

playlist = sp.user_playlist_create(user=user_id, name=f"{date_to_travel} Billboard 100", public=False)
print(playlist)

sp.playlist_add_items(playlist_id=playlist["id"], items=song_uris)