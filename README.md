# PolyglotUnity

A Unity3d plugin for Polyglot Localization that makes it possible to keep the localization editing in a google drive spreadsheet.

## Unity Version

Works with Unity 4.x, 5.x and 2017

Tested with several Unity Versions
* Unity 5.4.4f1
* Unity 5.5.2f1
* Unity 5.6.0f3
* Unity 2017.1.b1

## Feedback

We would 😍 to hear your opinion about this library. Please file an issue if there's something you would like to see improved.

If you use this library and are happy with it consider sending out a tweet mentioning [@agens](https://twitter.com/agens). This library is made with love by [Skjalg S. Mæhre](https://github.com/Skjalgsm).

[<img src="http://static.agens.no/images/agens_logo_w_slogan_avenir_medium.png" width="340" />](http://agens.no/)


## Current Features
- Download latest polyglot master sheet as CSV or TSV
- Import the downloaded file and parse it
- Managing localizations
	- Specify any number of csv or tsv files to parse
	- Select language
	- Select fallback language if the localization key does not exist in the currently selected language
	- Invokes event when language is changed
- Script for localizing UGUI Text component, TextMesh component and TextMesh Pro UGUI component
	- Auto completes localization key
	- Updates text and alignment of text (hebrew and arabic is left to right).
	- Supports parameters for localized strings such as "No {0} Selected".
- Script for saving selected language
	- Saves to PlayerPrefs
