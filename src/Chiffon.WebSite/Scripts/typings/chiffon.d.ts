
/// <reference path="jquery/jquery.d.ts"/>

// Built-in types

interface Window {
  jQuery: JQueryStatic;
  Chiffon: any;
}

// jquery.modal

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

// yepnope (très simplifié)

interface YepNopeOptions {
  load: string[];
  complete: () => void;
}

declare function yepnope(options: YepNopeOptions): void;
