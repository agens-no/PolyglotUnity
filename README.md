# PolyglotUnity

[<img src="http://i2.photobucket.com/albums/y39/karmis/Polyglot/parrot_medium_zpsomejqg3q.png" />]

A Unity3d plugin for Polyglot Localization, which is a project that translates games to many languages. This plugin makes it possible to automatically download the polyglot master spreadsheet and a custom game specific spreadsheet and parses it in a unity project.

https://docs.google.com/spreadsheets/d/17f0dQawb-s_Fd7DHgmVvJoEGDMH_yoSd8EYigrb0zmM/

## Unity Version

Works with Unity 4.x, 5.x, 2017.x and 2018.x

Tested with several Unity Versions
* Unity 5.4.4f1
* Unity 5.5.2f1
* Unity 5.6.0f3
* Unity 5.6.1f1
* Unity 2017.1.0b1
* Unity 2017.2.0f3
* Unity 2017.2.0p2
* Unity 2017.3.0b10

## Feedback

We would 😍 to hear your opinion about this library. Please file an issue if there's something you would like to see improved.

If you use this library and are happy with it consider sending out a tweet mentioning [@agens](https://twitter.com/agens). This library is made with love by [Skjalg S. Mæhre](https://github.com/Skjalgsm).

[<img src="http://static.agens.no/images/agens_logo_w_slogan_avenir_medium.png" width="340" />](http://agens.no/)

## First, you need to Configurate the Localization.
You do this by selecting the Configurate menu item from within Unity.
This will create the Localization asset for you if it is not set up yet and then select it.

![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/Configurate.png)

## Then you can specify the localization settings for the project.

![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/LocalizationAsset.png)

## Finally you can add Localized Text components to your Text objects

![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/LocalizedText.gif)

## Theres also a Language Dropdown script that automatically populates a Dropdown with the available languages
![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/LanguageDropdown.png)

## Create your own custom spreadsheet
Duplicate the polyglot master sheet and remove all the keys from line 7. The importer parses everything line after the term "polyglot", "PolyMaster" or "BEGIN".
![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/CopySheet.png)

## Add Sheet and docs id to the Localization Configuration
![alt tag](https://raw.githubusercontent.com/agens-no/PolyglotUnity/master/meta/CustomSheet.png)

## Current Features
- Download latest polyglot master sheet as CSV or TSV
- Download a custom localization sheet as CSV or TSV
    - If you use the same keys as the master sheet your keys will override the master sheet.
- Import the downloaded file and parse it
    - The sheet is parsed every time you play the game so you can iterate fast.
    - Can also be set to downloaded and parsed every time you play the game (optional)
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
- TextMesh Pro support
    - Make sure TMP_PRESENT is specified in Player Settings -> Scripting Define Symbols
    - [Additional info about dependencies](#additional-info-for-textmeshpro-integration)
- Arabic font type support
    - Download ArabicSupport.cs from https://github.com/Konash/arabic-support-unity
    - specify ARABSUPPORT_ENABLED in Player Settings -> Scripting Define Symbols

## Additional info for TextMeshPro integration

To use Polyglot with TextMeshPro from the Unity Package Manager (upm) you will need to add the dependency manually to the assembly definition files.

Versions tested:
* Unity 2018.1.3f1
* TextMeshPro 1.2.3 (from upm)

Open PolyglotScripts.asmdef and PolyglotEditor.asmdef in a text editor and manually add the dependency to Unity.TextMeshPro.

Your files should look like this:

PolyglotScripts.asmdef
```
{
    "name": "Polyglot.Scripts",
    "references": ["Unity.TextMeshPro"],
    "optionalUnityReferences": [],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false
}
```
PolyglotEditor.asmdef
```
{
    "name": "Polyglot.Editor",
    "references": [
        "Polyglot.Scripts",
        "Unity.TextMeshPro"
    ],
    "includePlatforms": [
        "Editor"
    ],
    "excludePlatforms": []
}
```
This makes the scripts find the dependency of TextMeshPro.