## PolyglotUnity ##
A unity3d plugin for Polyglot

Works with Unity 4.x and 5.x

Current Features
- Download latest polyglot master sheet
- Import the downloaded csv
- Script for managing localizations
	- Specify any number of csv files to parse
	- Select language
	- Select fallback language if the key does not exist in the currently selected language
	- Invokes event when language is changed
- Script for localizing Text and TextMesh
	- Auto completes localization key
	- Updates text and alignment of text (hebrew and arabix is left to right).
- Script for saving selected language
	- Saves to PlayerPrefs