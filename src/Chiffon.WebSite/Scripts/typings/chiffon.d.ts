
/// <reference path="jquery/jquery.d.ts"/>

interface ModalOptions {
  clickClose?: boolean;
  closeText?: string;
  escapeClose?: boolean;
  modalClass?: string;
  showClose?: boolean;
}

interface Modal {
  block(): void;
  center(): void;
  close(): void;
  hide(): void;
  open(): void;
  show(): void;
  unblock(): void;
}

interface JQuery {
  modal(options?: ModalOptions): any;
}

interface JQueryStatic {
  modal(elmt: any, options?: ModalOptions): Modal;
}
