# Text to speech for windows phone 7

This project is a library for translation and speaking tools from the Microsoft translator API. 
Each API call require s a token to obtained from the Microsoft Token Service. 
I built a separate library for this purpose https://github.com/bowofolaf/msttokenforwp7 and more information can be found on that page.

Currently only speaking in English is supported.

- Make sure to set the language property of the SpeechService object to "en" for English. This page shows all available languages http://msdn.microsoft.com/en-us/library/hh456380.aspx, a method is presently being developed to fetch this dynamically.

Features to be added

* Translation
* Multi-language support
* Less requests of new tokens
* All requirements are in the Windows Phone SDK and 'downloads' section.
