# PolyglotUnity
A unity3d plugin for Polyglot

All you need is a LocalizationManager in your scene. This manager keeps the references to the polyglot csv's and makes it possible to retreive localized string by calling

LocalizationManager.Get(string key);

It also contains some usefuls cripts for attaching to Text component/TextMesh component which enables localization of them. It also contains a language dropdown script which you can attach to any dropdown.

There is also an example options scene so that you can see how all of these things comes together.
