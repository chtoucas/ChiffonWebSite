/*jshint laxcomma: true, laxbreak:true*/
/*jslint nomen:true, white: true, todo: true*/

// TODO: null vs undefined
// TODO: que faire quand une resource ne se charge pas ?

/// <reference path="../../Scripts/typings/underscore/underscore.d.ts"/>
/// <reference path="yepnope.d.ts"/>

module Chiffon {
  'use strict';

  // Configuration de l'application.
  export interface Settings {
    baseUrl: string;
    locale: string;
    version: string;

    ajaxTimeout: number;
    debug: boolean;
  }

  export interface User {
    isAnonymous: boolean;
    isAuth: boolean;
  }

  export interface Context {
    settings: Settings;
    user: User;
  }

  export interface IComponentResolver {
    resolve(name: string): string[];
  }

  var defaults = {
      ajaxTimeout: 3000,
      debug: false
    },
    // Langues prises en charge.
    locales = ['fr', 'en'];

  export class App {
    settings: Settings;

    constructor(options: Settings) {
      var settings = _.defaults(options || {}, defaults);

      if (-1 === locales.indexOf(locale)) {
        throw new Error('The locale "' + locale + '" is not supported.');
      }
      if ('/' !== settings.baseUrl.substring(-1, 1)) {
        settings.baseUrl = settings.baseUrl + '/';
      }

      this.settings = settings;
    }

    require(name: string, onComplete: () => void) {
      var resources = this.resolver.resolve(name).map((src: string) => {
        return this.settings.baseUrl + src;
      });
      yepnope({ load: resources, complete: onComplete });
    }

    main(isAuth: boolean, fn: (app: Chiffon.Handler) => void) {
      var context = {
        settings: this.settings,
        user: {
          isAnonymous: !isAuth,
          isAuth: isAuth
        }
      };

      var resolver = new ComponentResolver(context);
      this.resolver = resolver;
      context.resolver = resolver;

      // TODO: Désactiver la suite pour les très petits écrans ?
      // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html

      this.require('jQuery', () => {
        // Si le chargement de jQuery a échoué, on dégage.
        if (undefined === jQuery) { return; }

        this.require('chiffon', () => { fn(new Handler(context)); });
      });
    }
  }

  export class Handler {
    constructor(public context: Context) { }

    handle(controllerName: string, actionName: string, params: any[]): void {
      throw new Error('You must override the "handleCore" method.');
    }
  }

  export interface Dependency {
    name: string;
    resource: () => string[];
  }

  export class ComponentResolver implements IComponentResolver {
    private registry = {};

    Create(dependencies: Dependency[]): IComponentResolver {
      var resolver = new ComponentResolver();
      foreach (dependency in dependencies) {
        resolver.register(dependency);
      }
      return resolver;
    }

    register(dependency: Dependency): void {
      registry[dependency.name] = dependency.resource;
    }

    resolve(name: string): string[] {
      var fn = registry[name];
      if (fn) {
        return fn();
      } else {
        throw new Error();
      }
    }
  }
}
