# Language Loader
This mod allows users to create translations for Stacklands.  No software development skills required.

# Important
Prior to 1.1.0 there was a bug where in certain situations, the selected language would not stay after the game restarted.
This has been fixed.

# Character/Symbol Notes
As of 2.0.0, custom fonts are now supported.  This will allow translations for languages that do not have the required glyphs (characters) in the game's default font.

The documentation to use custom fonts is in progress and will be completed shortly.

# Overview:
Here is a brief overview of what is involved.  

* Subscribe to this mod.
* Download and extract the language template.
* Use this mod to dump all of Stacklands' current translations to get the text and keys to translate.
* Translate the text.
* (Optional) Find a font that contains the target language's characters.
* Create a simple configuration file.
* Upload to Steam

# Tutorial
The full tutorial is available here:  https://github.com/NBKRedSpy/Stacklands-NewLanguageLoader/wiki/Create-a-New-Language

# Source Code
https://github.com/NBKRedSpy/Stacklands-NewLanguageLoader


# Credits:

Thanks to Damglador and skrybl for suggesting improvements and testing this mod.

Extra thanks to cyber and Damglador for providing instructions on how to create and load font assets at runtime.

# Localizations for This Mod's Text

* Ukrainian - Damglador

## Icons
[Gear icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/gear)

[Language icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/language)

# Change Log
## 2.1.0
* Changed custom font's asset name search to case insensitive.  
Unity's UI uses Pascal case, while the created bundles use lower case.

* Changed game to now always reboot if a custom font was selected.  Caused the translation to not load.

## 2.0.2
Added Damglador's Ukrainian translation for this mod's text.

## 2.0.1
When selecting a language that has a custom font, the game will reboot. 

## 2.0.0
Supports custom fonts to support specific language glyphs.

## 1.1.1
Added text localization for this mod. 
See localization.tsv for the keys.

## 1.1.0

Fix:  A disabled mod could prevent language packs from loading.

Added a "Set Language" button on the main menu.  Allows users to change the language by just searching for the icon, rather than going through several screens in a foreign language.

Moved new languages to the top of the languages for better visibility.
