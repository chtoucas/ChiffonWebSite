Notes de développement
======================

Tâches
------

En priorité,
- revoir les bidouilles effectuées spécialement pour la mise en place du site de démo
  * supprimer SHOWCASE et changer le conmportement du site en fonction d'un
    paramètre de configuration.
- DNS, il y a un problème avec www. et /simone/XXX

Travail de fond :
- basculer vers Owin.
- harmoniser les constantes dans HttpHandler, Views, Controllers.
- revoir toute la gestion des langues multiples :
  * VSModuleContext (activer en localhost) + session ;
  * utiliser `CurrentCulture` plutôt que `CurrentUICulture`.
- refaire entièrement ChiffonEnv... et SiteMap.
- Eviter HttpContext.Current. Cf. http://odetocode.com/articles/112.aspx
- renommer Login -> Connect ;
- revoir OntologyFilter ;
- ré-activer robots.txt ;
- utiliser Attribute Routing ;
- finir `MailMerge` ;
- rajouter les scripts de déploiement aux packages ;
- encrypter les chaînes de connection ;
- utiliser des tableaux de caractères plutôt qu'une chaîne de caractères pour le mdp.

Création d'un package
---------------------

1. changer le numéro de version dans `src\AssemblyInfo.Common.cs` et `VersionInfo.xml` ;
2. lancer `build.cmd`. Le package sera créé dans le répertoire `artefacts` ;
3. tagger la nouvelle version :
```
git tag -a 1.18.0 -m 'Version 1.18.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin --tags
```

Mise à jour des librairies JavaScript & CSS
-------------------------------------------

À chaque nouvelle version d'une librairie JavaScript, si nécessaire, mettre à jour :
- `Gruntfile.js` ;
- `src\assets\js\chiffon.js` ;
- `src\assets\js\chiffon.views.js` ;
- `src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml` ;
- `src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml` ;

et pour les feuilles de style, on fera attention à :
- `src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml`.

Après coup, aller dans Visual Studio, supprimer les anciennes version et
référencer les nouvelles.

En ce qui concerne nodejs :
- lancer `tools\npm-check-updates` pour vérifier la disponibilité de nouvelles versions ;
- puis `tools\npm-check-updates -u` pour mettre à jour le fichier `package.json`
- et enfin `npm install` pour la mise à jour effective.

On peut aussi utiliser `npm update --save-dev`.

Dépendances mises à jour via nodejs :
- [Lo-Dash](https://lodash.com/). Attention, la nouvelle version ne sera
  disponible qu'après avoir exécuté grunt.
- [less.js](http://lesscss.org/). Attention, il ne faut pas oublier de faire
  une copie des fichiers dans `src\Chiffon.WebSite\assets\vendor`.

Dépendances mises à jour manuellement :
- [FastClick](https://github.com/ftlabs/fastclick), uniquement le js ;
- [jQuery](https://jquery.com/download/), les trois fichiers js, min.js et min.map ;
- [jQuery.validate](http://jqueryvalidation.org/), les deux fichiers js, min.js
  ainsi que les fichiers de langue ;
- [NProgress](http://ricostacruz.com/nprogress/). Attention, il faut mettre
  à jour le js et le css ;
- [jQuery.Waypoints](https://github.com/imakewebthings/waypoints), les deux
  fichiers js et min.js ;
- [normalize.css](http://necolas.github.io/normalize.css/).

Dépendances ne nécessitant pas de mises à jour :
- [yepnope](http://yepnopejs.com/), _plus de mises à jour disponibles_ ;
- [H5BP](https://github.com/h5bp/html5-boilerplate), internalisé à partir de la version 4.3 ;
- jquery.microdata, internalisé (je ne retrouve même plus la référence d'origine).

CSS & JavaScript
----------------

Pour compiler les CSS et JS, lancer `tools\grunt.cmd`.
Supprimer après coup les anciennes versions dans `src\assets`.

Quelques recommandations :
- n'utiliser que des classes dans les CSS, éviter les IDs autant que faire se peut ;
- utiliser uniquement des IDs dans les sélecteurs jQuery.

Tests d'interface
-----------------

Analyse des performances en conditions réelles :
- YSlow Extension
- PageSpeed Extension
- [PageSpeed Insights](http://developers.google.com/speed/pagespeed/insights/)

Tester l'interface avec des navigateurs désués :
- [Modern IE](http://www.modern.ie/)
- [IE Tester](http://my-debugbar.com/wiki/IETester/HomePage)
- [Windows XP Mode](http://windows.microsoft.com/en-us/windows7/products/features/windows-xp-mode)
