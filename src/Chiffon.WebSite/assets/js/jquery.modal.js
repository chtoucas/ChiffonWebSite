/*global $*/

/*
 * Adapté de http://github.com/kylefox/jquery-modal
 */
(function(window, undef) {
  'use strict';

  var ESC_KEYCODE = 27;
  var MODAL_KEYDOWN_EVENT = 'keydown.modal';
  //var MODAL_CLOSE_EVENT = 'modal:close';

  var document = window.document;
  var location = window.location;
  var current = undef;

  $.modal = function(elmt, options) {
    var target;

    if (!elmt.is('a')) {
      return;
    }

    // On ferme toutes les modales.
    $.modal.close();

    this.options = $.extend({}, $.modal.defaults, options);

    this.$body = $('body');
    this.$elm = $('<div>');
    this.$body.append(this.$elm);

    target = elmt.attr('href');

    $.get(target).done(function(data) {
      if (!current) { return; }

      current.$elm.empty().append(data);
      //.on(MODAL_CLOSE_EVENT, function(e, modal) {
      //  modal.elm.remove();
      //});
      current.open();
    }).fail(function() {
      // NB: ne pas utiliser location.href = target car cela ne marche pas
      // dans les anciens IE.
      // TODO: Cela ne semble prendre en compte les erreurs 500+
      location = target;
    });
  };

  $.modal.prototype = {
    constructor: $.modal,

    open: function() {
      this.block();
      this.show();

      if (this.options.escapeClose) {
        $(document).on(MODAL_KEYDOWN_EVENT, function(e) {
          if (ESC_KEYCODE === e.which) {
            $.modal.close();
          }
        });
      }

      if (this.options.clickClose) {
        this.$overlay.on('click', $.modal.close);
      }
    },

    close: function() {
      this.unblock();
      this.hide();
      $(document).off(MODAL_KEYDOWN_EVENT);
    },

    block: function() {
      this.$overlay = $('<div class=overlay></div>').appendTo(this.$body).show();
    },

    unblock: function() {
      this.$overlay.remove();
    },

    show: function() {
      if (this.options.showClose) {
        this.closeButton = $('<a href="#" rel="modal:close" class="close ir">' + this.options.closeText + '</a>');
        this.$elm.append(this.closeButton);
      }

      this.$elm.addClass(this.options.modalClass + ' current');
      this.$elm.show();
    },

    hide: function() {
      if (this.closeButton) {
        this.closeButton.remove();
      }

      this.$elm.removeClass('current');

      this.$elm.hide();
    }
  };

  $.modal.close = function(e) {
    if (undef === current) { return; }
    if (e) { e.preventDefault(); }

    current.close();
    var that = current.$elm;
    current = undef;
    return that;
  };

  $.modal.defaults = {
    clickClose: true,
    closeText: 'Close',
    escapeClose: true,
    modalClass: 'modal',
    showClose: true
  };

  $.fn.modal = function(options) {
    if (1 === this.length) {
      current = new $.modal(this, options);
    }

    return this;
  };

  // Automatically bind links with rel="modal:close" to, well, close the modal.
  $(document).on('click.modal', 'a[rel="modal:close"]', $.modal.close);
})(this);

