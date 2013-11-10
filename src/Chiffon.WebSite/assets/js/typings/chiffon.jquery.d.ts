
/// <reference path="../../../Scripts/typings/jquery/jquery.d.ts"/>

interface JQueryWatermarkOptions {
  wrapperStart?: string;
  wrapperEnd?: string;
}

interface JQuery {
  external(): any;
  watermark(watermark?: string, options?: JQueryWatermarkOptions): any;
}
