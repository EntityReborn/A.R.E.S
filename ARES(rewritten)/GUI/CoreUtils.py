#A file containing all the core modules in relation to the GUI itself

#Importing reqired modules
import requests, os, json, pymsgbox, datetime, shutil,re,traceback, hashlib, subprocess
from base64 import b64encode
from clint.textui import progress
#Importing custom ARES modules
from LogUtils import DecideAssetURL
def InitCore():
    global BaseD
    BaseD = os.getcwd()
#Fetches special thanks from our pastebin
def GetSpecialThanks():
    try:
        SPTX = requests.get("https://raw.githubusercontent.com/LargestBoi/A.R.E.S/main/VersionHashes/SpecialThanks.txt", timeout=10).text
    except:
        SPTX = "Couldn't Connect!"
    return SPTX
#Verifys the validity of the key entered
def KeyCheck(key):
    try:
        url = "https://api.avataruploader.tk/verifykey"
        headers = {
            'accept': 'application/json',
            'user-agent': key,
        }
        response = requests.get(url, headers=headers)
        return response.json()
    except:
        traceback.print_exc()
        return "Error with key check!"
#Retruns true if the application has completed its first time setup
def IsSetup():
    if not os.path.exists("Logs"):
        os.mkdir("Logs")
    if os.path.exists("Latest.log"):
        shutil.move("Latest.log",f'Logs\\{str(datetime.datetime.now()).replace(":","-")}.txt')
    if os.path.exists("Settings.json"):
        return True
    return False
#Gets the settings information
def GetSettings():
    with open("Settings.json", "r+") as s:
        return json.loads(s.read())
#Saves the settings provided to the settings file
def SaveSettings(settings):
    with open("Settings.json", "w+") as s:
        s.write(json.dumps(settings, indent=4))
#Logs events
def EventLog(Data):
    log = f'{str(datetime.datetime.now())} | {Data}'
    with open(f"{BaseD}\\Latest.log", "a+") as l:
        l.write(f'{log}\n')
    return log
def ErrorLog(Key,Data):
    try:
        headers = {
            'User-Agent': str(Key).replace(" ",""),
            'Content-Type': 'application/json'
        }

        data = {"key": Key, "error": b64encode(Data.encode('utf-8')).decode('utf-8')}
        response = requests.post('http://api.avataruploader.tk/submiterror', headers=headers, json=data)
        if response.json()['status'] == "success":
            EventLog("Error sent to API")
    except:
        pass
#Function to download VRCAs
def DownloadVRCA(PC,Q):
    os.startfile(DecideAssetURL(PC,Q))
#Function to download VRCAs for internal use
def DownloadVRCAFL(PC,Q):
    payload = ""
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
        "Content-Type": "application/json",
        "Bypass-Tunnel-Reminder": "bypass"
    }
    data = requests.request("GET", DecideAssetURL(PC,Q), data=payload, headers=headers, stream=True)
    # Writes content to file
    with open("HOTSWAP\\Avatar.vrca", "wb") as v:
        v.write(data.content)

def SetAviImage(ImageURL):
    payload = ""
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
        "Content-Type": "application/json",
        "Bypass-Tunnel-Reminder": "bypass"
    }
    data = requests.request("GET", ImageURL, data=payload, headers=headers, stream=True)
    # Writes content to file
    with open(f"{BaseD}\\HSB\\Assets\\Logo.png", "wb") as v:
        v.write(data.content)
def LoadLog():
    try:
        Pattern = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nPC Asset URL:(.*)\nQuest Asset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nUnity Version:(.*)\nRelease Status:(.*)\nTags:(.*)"
        # Setup logs to be read
        with open(f"{BaseD}\\Log.txt", "r+", errors="ignore") as lf:
            Logs = lf.read()
            # Find all logs via pattern
            Log = re.findall(Pattern, Logs)
            return Log
    except:
        EventLog("Error executing load log to upload avatars:\n" + traceback.format_exc())

#Cleanly exits ARES and any other possibly conflicting software
def CleanExit():
    try:
        os.system('taskkill /F /im "ARES.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "HOTSWAP.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "Unity Hub.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "Unity.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "AssetRipperConsole.exe"')
    except:
        pass