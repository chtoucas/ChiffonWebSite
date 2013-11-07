
/// <reference path="jquery/jquery.d.ts"/>

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
  modal(options?: ModalOptions): any;
}

interface JQueryStatic {
  modal(elmt: any, options?: ModalOptions): Modal;
}
