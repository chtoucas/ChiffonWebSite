
/// <reference path="jquery/jquery.d.ts"/>

interface WatermarkOptions {
  wrapperStart?: string;
  wrapperEnd?: string;
}

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
  external(): any;
  modal(options?: ModalOptions): any;
  watermark(watermark?: string, options?: WatermarkOptions): any;
}

interface JQueryStatic {
  modal(elmt: any, options?: ModalOptions): Modal;
}
