import requests, json, time

if __name__ == '__main__':
    text = """Text to speech technology allows you to convert text of unlimited sizes to humanlike voice audio files!"""
    apikey = "1cc4d2218bmshe456bc8d87002fap159ec0jsnd918b3538dc3" # get your free API key from https://rapidapi.com/k_1/api/large-text-to-speech/
    filename = "test-file.wav"

    # POST
    headers = {'content-type': "application/json", 'x-rapidapi-host': "large-text-to-speech.p.rapidapi.com", 'x-rapidapi-key': apikey}
    response = requests.request("POST", "https://large-text-to-speech.p.rapidapi.com/tts", data=json.dumps({"text": text}), headers=headers)
    print(response.text)
    id = json.loads(response.text)['id']
    eta = json.loads(response.text)['eta']
    print(f'Waiting {eta} seconds for the job to finish...')
    time.sleep(eta)

    # GET 1
    response = requests.request("GET", "https://large-text-to-speech.p.rapidapi.com/tts", headers=headers, params={'id': id})
    while "url" not in json.loads(response.text):
        response = requests.request("GET", "https://large-text-to-speech.p.rapidapi.com/tts", headers=headers, params={'id': id})
        print(f'Waiting some more...')
        time.sleep(3)

    # GET 2
    url = json.loads(response.text)['url']
    response = requests.request("GET", url)
    with open(filename, 'wb') as f:
        f.write(response.content)
    print(f'File saved to {filename} ! \nOr download here: {url}')