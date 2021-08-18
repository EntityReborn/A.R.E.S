import os, sys, requests, json, threading, shutil, winshell, traceback, win32com
from win32com.client import Dispatch
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(381, 270)
        uic.loadUi('untitled.ui', self)  # Load the .ui file
        self.show()
        self.SelVRC = self.findChild(QtWidgets.QPushButton, 'SelVRC')
        self.SelVRC.clicked.connect(self.SelVRC1)
        self.SelUnity = self.findChild(QtWidgets.QPushButton, 'SelUnity')
        self.SelUnity.clicked.connect(self.SelUnity1)
        self.SelUnity.setEnabled(False)
        self.Install = self.findChild(QtWidgets.QPushButton, 'Install')
        self.Install.clicked.connect(self.Install1)
        self.Install.setEnabled(False)
        self.LogOwnAvisCB = self.findChild(QtWidgets.QCheckBox, 'LogOwnAvisCB')
        self.LogFriendsAvisCB = self.findChild(QtWidgets.QCheckBox, 'LogFriendsAvisCB')
        self.LogToConsoleCB = self.findChild(QtWidgets.QCheckBox, 'LogToConsoleCB')
        self.AllowAPICB = self.findChild(QtWidgets.QCheckBox, 'AllowAPICB')
        self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'progressBar')
        self.ProgBar.setValue(0)
    def SelVRC1(self):
        self.VRCPath = QFileDialog.getOpenFileName(self, 'Select VRChat.exe', 'VRChat',"EXE Files (*.exe)")[0].replace("/VRChat.exe", "")
        self.ProgBar.setValue(6)
        self.UPath = ""
        self.SelUnity.setEnabled(True)
        self.Install.setEnabled(True)
    def SelUnity1(self):
        self.UPath = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
        self.ProgBar.setValue(12)
    def Install1(self):
        threading.Thread(target=self.Install2, args=()).start()
    def Install2(self):
        try:
            if os.path.isdir(self.VRCPath + "/AvatarLog"):
                self.ProgBar.setValue(18)
                try:
                    self.ProgBar.setValue(24)
                    with open(self.VRCPath + "/AvatarLog/Log.txt", "r+", errors="ignore") as l:
                        self.OldLog = l.read()
                    shutil.rmtree(self.VRCPath + "/AvatarLog")
                    os.remove(self.VRCPath + "/Leaf.xNet.dll")
                    os.remove(self.VRCPath + "/Mods/AvatarLogger.dll")
                except:
                    pass
            if os.path.isdir(self.VRCPath + "/MOD"):
                shutil.rmtree(self.VRCPath + "/MOD")
            if os.path.isdir(self.VRCPath + "/GUI"):
                shutil.rmtree(self.VRCPath + "/GUI")
            self.ProgBar.setValue(30)
            DLLink = requests.get("https://pastebin.com/raw/Q0w5ttLH", timeout=10).text
            self.GUIURL = DLLink.split("|")[0]
            self.ModURL = DLLink.split("|")[1]
            self.ProgBar.setValue(36)
            os.system("curl -L " + self.GUIURL + " > GUI.rar")
            self.ProgBar.setValue(42)
            os.system("curl -L " + self.ModURL + " > MOD.rar")
            self.ProgBar.setValue(48)
            os.mkdir("MOD")
            os.system("UnRAR.exe x MOD.rar MOD")
            self.ProgBar.setValue(54)
            os.mkdir("GUI")
            os.system("UnRAR.exe x GUI.rar GUI")
            self.ProgBar.setValue(60)
            with open("MOD/AvatarLog/Config.json", "r+", errors="ignore") as c:
                self.ModConfig = json.loads(c.read())
            self.LOA = self.LogOwnAvisCB.isChecked()
            self.ModConfig["LogOwnAvatars"] = self.LOA
            self.LFA = self.LogFriendsAvisCB.isChecked()
            self.ModConfig["LogFriendsAvatars"] = self.LFA
            self.LTC = self.LogToConsoleCB.isChecked()
            self.ModConfig["LogToConsole"] = self.LTC
            self.AA = self.AllowAPICB.isChecked()
            self.ModConfig["SendToAPI"] = self.AA
            with open("MOD/AvatarLog/Config.json", "w+", errors="ignore") as c:
                c.write(json.dumps(self.ModConfig, indent=4))
            with open("GUI/Settings.json", "r+", errors="ignore") as c:
                self.GUISettings = json.loads(c.read())
            self.GUISettings["Avatar_Folder"] = self.VRCPath
            self.GUISettings["Unity_Exe"] = self.UPath
            with open("GUI/Settings.json", "w+", errors="ignore") as c:
                c.write(json.dumps(self.GUISettings, indent=4))
            self.ProgBar.setValue(66)
            shutil.move("GUI", self.VRCPath)
            self.ProgBar.setValue(72)
            shutil.move("MOD/Leaf.xNet.dll", self.VRCPath)
            shutil.move("MOD/AvatarLog", self.VRCPath)
            shutil.move("MOD/Mods/AvatarLogger.dll", self.VRCPath + "/Mods")
            try:
                with open(self.VRCPath + "/AvatarLog/Log.txt", "w+", errors="ignore") as l:
                    l.write(self.OldLog)
                self.ProgBar.setValue(78)
            except:
                pass
            self.ProgBar.setValue(84)
            if os.path.isdir("MOD"):
                shutil.rmtree("MOD")
            if os.path.isdir("GUI"):
                shutil.rmtree("GUI")
            os.remove("MOD.rar")
            os.remove("GUI.rar")
            self.DT = winshell.desktop()
            self.path = os.path.join(self.DT, 'Avatar Logger GUI.lnk')
            self.target = self.VRCPath + "/GUI/main.exe"
            self.wDir = self.VRCPath + "/GUI"
            self.icon = self.VRCPath + "/GUI/main.exe"
            self.shell = Dispatch('WScript.Shell')
            self.shortcut = self.shell.CreateShortCut(self.path)
            self.shortcut.Targetpath = self.target
            self.shortcut.WorkingDirectory = self.wDir
            self.shortcut.IconLocation = self.icon
            self.shortcut.save()
            self.ProgBar.setValue(100)
        except:
            traceback.print_exc()
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
# app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application