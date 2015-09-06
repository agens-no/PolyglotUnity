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
- Script for localizing UGUI Text component, TextMesh component and TextMesh Pro UGUI component
	- Auto completes localization key
	- Updates text and alignment of text (hebrew and arabic is left to right).
	- Supports parameters for localized strings such as "No {0} Selected".
- Script for saving selected language
	- Saves to PlayerPrefs
