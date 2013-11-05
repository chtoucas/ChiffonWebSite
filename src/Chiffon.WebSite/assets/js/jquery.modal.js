;

/*
 * TODO:
 * - configuration d'ajax (timeout & co)
 * - slider quand on reste dans la modale ?
 * - meilleur indicateur de chargement ?
 * - utiliser RxJS ?
 */
/*
 * A simple jQuery modal (http://github.com/kylefox/jquery-modal)
 * Version 0.5.4
 */
(function($, undef) {
  'use strict';

  var current = undef;

  $.modal = function(elmt, options) {
    var remove, target;

    $.modal.close(); // Close any open modals.

    this.options = $.extend({}, $.modal.defaults, options);

    this.$body = $('body');

    if (elmt.is('a')) {
      target = elmt.attr('href');
      if (/^#/.test(target)) {
        // Select element by id from href

        this.$elm = $(target);
        if (this.$elm.length !== 1) return undef;
        this.open();
      } else {
        // AJAX

        this.$elm = $('<div>');
        this.$body.append(this.$elm);
        remove = function(event, modal) { modal.elm.remove(); };

        $.get(target).done(function(html) {
          if (!current) return;
          current.$elm.empty().append(html).on($.modal.CLOSE, remove);
          current.open();
        }).fail(function() {
          //
        });
      }
    } else {
      this.$elm = elmt;
      this.open();
    }
  };

  $.modal.prototype = {
    constructor: $.modal,

    open: function() {
      this.block();
      this.show();

      if (this.options.escapeClose) {
        $(document).on('keydown.modal', function(e) {
          if (e.which == 27) $.modal.close();
        });
      }

      if (this.options.clickClose) this.blocker.click($.modal.close);
    },

    close: function() {
      this.unblock();
      this.hide();
      $(document).off('keydown.modal');
    },

    block: function() {
      this.blocker = $('<div class="overlay"></div>');
      this.$body.append(this.blocker);
      this.blocker.show();
    },

    unblock: function() {
      this.blocker.remove();
    },

    show: function() {
      if (this.options.showClose) {
        this.closeButton = $('<a href="#" rel="modal:close" class=close>' + this.options.closeText + '</a>');
        this.$elm.append(this.closeButton);
      }
      this.$elm.addClass(this.options.modalClass + ' current');
      this.center();
      this.$elm.show();
    },

    hide: function() {
      if (this.closeButton) this.closeButton.remove();
      this.$elm.removeClass('current');

      this.$elm.hide();
    },

    center: function() {
      this.$elm.css({ top: '50%', marginTop: -(this.$elm.outerHeight() / 2) });
    }
  };

  $.modal.close = function(e) {
    if (!current) return;
    if (e) e.preventDefault();
    current.close();
    var that = current.$elm;
    current = undef;
    return that;
  };

  $.modal.defaults = {
    escapeClose: true,
    clickClose: true,
    closeText: 'Close',
    modalClass: 'modal',
    showClose: true
  };

  // Event constants
  $.modal.OPEN = 'modal:open';
  $.modal.CLOSE = 'modal:close';

  $.fn.modal = function(options) {
    if (this.length === 1) {
      current = new $.modal(this, options);
    }
    return this;
  };

  // Automatically bind links with rel="modal:close" to, well, close the modal.
  $(document).on('click.modal', 'a[rel="modal:close"]', $.modal.close);
})(this.jQuery);

