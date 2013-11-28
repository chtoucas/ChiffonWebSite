TODO
====

* Pages d'erreur.
* Finaliser l'outil de déploiement.
* L'enregistrement SPF semble incorrect.

* link alternate (pour l'anglais & vice versa)


Améliorer le design des CSS & des JS :
* http://smacss.com/
* http://javascriptweblog.wordpress.com/2011/05/31/a-fresh-look-at-javascript-mixins/

* Cf. CommerceShop

Pour les déploiements, peut-être s'inspirer de :
* https://github.com/AppVeyor/AppRolla
* http://www.infoq.com/articles/AppVeyor-CI
* http://psappdeploytoolkit.codeplex.com/
* http://octopusdeploy.com/

Utiliser Google Closure Tools (IronPython via NuGet ?)

Remplacer YUICompressor par une alternative .NET (?) et Ã©ventuellement activer de nouvelles
optimisations Web (minification HTML, sprites) :
* http://webgrease.codeplex.com/
* http://bundletransformer.codeplex.com/
* http://webmarkupmin.codeplex.com/
* https://github.com/ServiceStack/Bundler
* http://ajorkowski.github.io/NodeAssets/
* https://github.com/paulcbetts/SassAndCoffee

Vérifier les CSS & JS :
* JSLint
- http://jslintnet.codeplex.com/
- http://jslint4vs2010.codeplex.com/
- http://nemetht.wordpress.com/2013/05/15/integrate-jslintjshint-into-the-msbuild-the-easy-way/
- http://sedodream.codeplex.com/

Migrer vers ASP.Net MVC 5 :
* http://www.asp.net/mvc/tutorials/mvc-5/how-to-upgrade-an-aspnet-mvc-4-and-web-api-project-to-aspnet-mvc-5-and-web-api-2
* http://madskristensen.net/post/url-rewrite-may-break-aspnet-razor-3

Améliorer l'internationalisation :
* http://afana.me/post/aspnet-mvc-internationalization.aspx
* http://afana.me/post/aspnet-mvc-internationalization-store-strings-in-database-or-xml.aspx
* http://attributerouting.net/

MSBuild :
* FxCop (via MSBuild.ExtensionPack)
* StyleCop (via MSBuild.ExtensionPack)

Utiliser https://github.com/kozy4324/grunt-concat-sourcemap

RxJS
http://haacked.com/archive/2012/04/08/reactive-extensions-sample.aspx
http://blogs.msdn.com/b/interoperability/archive/2013/02/05/netflix-solving-big-problems-with-reactive-extensions-rx.aspx

I18N
http://www.mediawiki.org/wiki/Localisation#Internationalization_hints

NOTES
-----

# NB: Pour une explication des erreurs jslint, cf. http://jslinterrors.com/

# WARNING: node-jslint utilise console.log et lorsqu'on redirige la sortie dans une fenêtre
# PowerShell, le fichier cible est vide :-( Apparemment je ne suis pas le seul à rencontrer
# ce problème :
#   http://stackoverflow.com/questions/9846326/node-console-log-behavior-and-windows-stdout
# Pour contourner ce problÃ¨me, on utilise donc un script intermÃ©diaire :
# .\node_modules\.bin\jslint.cmd %* >> .\reports\jslint.log

#FIXME .\jslint.cmd --maxerr 100 $source