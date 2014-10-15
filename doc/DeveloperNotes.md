CSS & JavaScript
================

* N'utiliser que des classes dans les CSS, éviter les IDs autant que faire se peut.
* Utiliser uniquement des IDs dans les sélecteurs jQuery.


Tests d'interface
=================

Analyse des performances en conditions réelles :
* YSlow Extension
* PageSpeed Extension
* [PageSpeed Insights](http://developers.google.com/speed/pagespeed/insights/)

Interface :
* [Modern IE](http://www.modern.ie/)
* [IE Tester](http://my-debugbar.com/wiki/IETester/HomePage)
* [Windows XP Mode](http://windows.microsoft.com/en-us/windows7/products/features/windows-xp-mode)


Mise à jour des librairies JavaScript
=====================================

Màj package.json en changeant toutes les version en "*" (Hum non, ce n'est pas la bonne manière
de faire) :
- npm update --save-dev

À chaque nouvelle version de yepnope, Lo-Dash ou FastClick, mettre à jour :
- src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml

Pour less, màj
- src\Chiffon.WebSite\Views\Widget\StyleSheet.Debug.cshtml

Dans tous les cas, màj
- chiffon.js & chiffon.views.js
- Gruntfile.js



Gruntfile.js

src\assets\js\chiffon.js
src\assets\js\chiffon.jquery.js
src\assets\js\chiffon.views.js
src\Chiffon.WebSite\Views\Component\JavaScript.Debug.cshtml
src\Chiffon.WebSite\Views\Component\JavaScript.Release.cshtml