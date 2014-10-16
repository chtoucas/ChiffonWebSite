/*global  Chiffon*/

// On n'exécute automatiquement Chiffon.main que si args est défini. Cette restriction est utile
// car elle permet d'utiliser aussi ce fichier comme une librairie (par ex pour les tests).
// Pour les anciens navigateurs, on désactive complètement JavaScript.
// La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
//  Cf. http://kangax.github.io/es5-compat-table/
// TODO: Désactiver aussi pour les petits écrans ?
//  Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
if (undefined !== this.args && typeof Function.prototype.bind === 'function') {
  Chiffon.main(this.args);
}
