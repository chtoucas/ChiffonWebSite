
/// <reference path="../../../Scripts/typings/jquery/jquery.d.ts"/>

// jquery.modal

interface JQueryModalOptions {
  clickClose?: boolean;
  closeText?: string;
  escapeClose?: boolean;
  modalClass?: string;
  showClose?: boolean;
}

interface JQueryModal {
  block(): void;
  center(): void;
  close(): void;
  hide(): void;
  open(): void;
  show(): void;
  unblock(): void;
}

interface JQuery {
  modal(options?: JQueryModalOptions): any;
}

interface JQueryStatic {
  modal(elmt: any, options?: JQueryModalOptions): JQueryModal;
}
