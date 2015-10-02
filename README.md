# Flow (Short for Unity Workflow)

![Logo](http://flowerpickinggiants.de/art/flower-logo-small.png)

This is a small collection of Editor Extensions I wrote during my work with Unity.
Mainly it makes you stick more to the keyboard when assembling scenes or navigating in Unity.

## Installation

### Asset Store Package
Asset Store Package is on the way. Will place link here if available.

### Git

#### Submodule if using Git for your project

Use ```git submodule add https://github.com/HatiEth/Flow-Unity3D.git Assets/Flow/``` in the root folder of your Project.

#### "Simple way"
Simply ```git clone``` this repository into your Unity3D project, or a separate and copy the files over to Assets/Flow/ folder in your Unity Project.

#### Just download the zip
Simply download the Zip and extract its content into Assets/Flow/ (which you have to create) in your Unity Project.

## Content

Current Extensions:
* QuickScene
* QuickFile

### QuickScene (Cmd/Ctrl+Shift+O)

Can also be found under Window/QuickScene - but why would you ...
Shortcut: (Cmd/Ctrl+Shift+O)

#### IMPORTANT

Do not dock this one as it gets closed on selection anyway.

#### Description

Small Dialog to help you quickly switch between scenes. 
Especially useful if you use scenes for like editing prefabs.

Keyboard Control:
Up-Arrow/Down-Arrow to move selection.
Enter to Select.
Esc to close.

### QuickFile (Cmd/Ctrl+Shift+E)

Shortcut: (Cmd/Ctrl+Shift+E)

#### IMPORTANT

Do not dock this one as it gets closed on selection anyway.


#### Description

Small Dialog to help you easily open & select files in the inspector / open a file.
Search bar allows some barebone Regex (Sadly, "\*" does not work :/ )

Keyboard Control is also enabled:

Up-Arrow/Down-Arrow to move selection.
Enter to Select.
Shift+Enter to Open the file with representative Tool.
Esc to close.



## License

MIT LICENSE
