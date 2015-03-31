TODO
----

- Remove entirely the links for HTTP 204
- Showcase: disable inscription
- DNS, there is a problem with www. and /XXX
- Update mail and add web site for V
  Remove mail and web site for the others
- Mail redirection
- Disable GA
- Change "virtualdirectory" to "simone"
- Remove all the SHOWCASE stuff -> Configuration
- Re-enable language switch
- Use CurrentCulture instead of CurrentUICulture for localization

- Re-enable robots.txt
- Migrate to Attribute Routing
- Finish mail merge


* rajouter aux package les scripts de déploiement
* encrypter les chaînes de connection
* compilation pour de multiples plateformes ?
* RestorePackages ?
* IISExpress, configSource

* Migrate to Attribute Routing
* Multi-langues
  - VSModuleContext (activer en localhost) + session lang
* chemins dans httpErrors
* désactivater la création de compte et l'envoi de mail ?
* virer SHOWCASE et déterminer via la configuration si on est dans un
  répertoire virtuel.

Mise en production
------------------

1. Mettre à jour la version dans `src\AssemblyInfo.Common.cs` et `VersionInfo.xml`.
2. Lancer `build.cmd`.

Mise à jour des librairies JavaScript
-------------------------------------

Màj package.json:
- `tools\npm-check-updates`
- `npm update --save-dev`

À chaque nouvelle version de yepnope, Lo-Dash ou FastClick, mettre à jour :
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`

Pour less, màj
- `src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml`

Dans tous les cas, màj
- `chiffon.js` & `chiffon.views.js`
- `Gruntfile.js`

`Gruntfile.js`
`src\assets\js\chiffon.js`
`src\assets\js\chiffon.jquery.js`
`src\assets\js\chiffon.views.js`
`src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml`
`src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml`
                                                
CSS & JavaScript
----------------

* N'utiliser que des classes dans les CSS, éviter les IDs autant que faire se peut.
* Utiliser uniquement des IDs dans les sélecteurs jQuery.

Tests d'interface
-----------------

Analyse des performances en conditions réelles :
* YSlow Extension
* PageSpeed Extension
* [PageSpeed Insights](http://developers.google.com/speed/pagespeed/insights/)

Interface :
* [Modern IE](http://www.modern.ie/)
* [IE Tester](http://my-debugbar.com/wiki/IETester/HomePage)
* [Windows XP Mode](http://windows.microsoft.com/en-us/windows7/products/features/windows-xp-mode)