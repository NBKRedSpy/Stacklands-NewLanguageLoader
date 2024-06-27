[h1]Language Loader[/h1]

This mod allows users to create translations for Stacklands.  No software development skills required.

[h1]Important[/h1]

Prior to 1.1.0 there was a bug where in certain situations, the selected language would not stay after the game restarted.
This has been fixed.

[h1]Character/Symbol Notes[/h1]

As of 2.0.0, custom fonts are now supported.  This will allow translations for languages that do not have the required glyphs (characters) in the game's default font.

The documentation to use custom fonts is in progress and will be completed shortly.

[h1]Overview:[/h1]

Here is a brief overview of what is involved.
[list]
[*]Subscribe to this mod.
[*]Download and extract the language template.
[*]Use this mod to dump all of Stacklands' current translations to get the text and keys to translate.
[*]Translate the text.
[*](Optional) Find a font that contains the target language's characters.
[*]Create a simple configuration file.
[*]Upload to Steam
[/list]

[h1]Tutorial[/h1]

The full tutorial is available here:  https://github.com/NBKRedSpy/Stacklands-NewLanguageLoader/wiki/Create-a-New-Language

[h1]Source Code[/h1]

https://github.com/NBKRedSpy/Stacklands-NewLanguageLoader

[h1]Credits:[/h1]

Thanks to Damglador and skrybl for suggesting improvements and testing this mod.

Extra thanks to cyber and Damglador for providing instructions on how to create and load font assets at runtime.

[h1]Localizations for This Mod's Text[/h1]
[list]
[*]Ukrainian - Damglador
[/list]

[h2]Icons[/h2]

[url=https://www.flaticon.com/free-icons/gear]Gear icons created by Freepik - Flaticon[/url]

[url=https://www.flaticon.com/free-icons/language]Language icons created by Freepik - Flaticon[/url]

[h1]Change Log[/h1]

[h2]2.1.0[/h2]
[list]
[*]
Changed custom font's asset name search to case insensitive.
Unity's UI uses Pascal case, while the created bundles use lower case.
[*]
Changed game to now always reboot if a custom font was selected.  Caused the translation to not load.
[/list]

[h2]2.0.2[/h2]

Added Damglador's Ukrainian translation for this mod's text.

[h2]2.0.1[/h2]

When selecting a language that has a custom font, the game will reboot.

[h2]2.0.0[/h2]

Supports custom fonts to support specific language glyphs.

[h2]1.1.1[/h2]

Added text localization for this mod.
See localization.tsv for the keys.

[h2]1.1.0[/h2]

Fix:  A disabled mod could prevent language packs from loading.

Added a "Set Language" button on the main menu.  Allows users to change the language by just searching for the icon, rather than going through several screens in a foreign language.

Moved new languages to the top of the languages for better visibility.
