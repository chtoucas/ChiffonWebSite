/// <reference path="../../Scripts/typings/jquery/jquery.d.ts" />
interface WatermarkOptions {
    wrapperStart?: string;
    wrapperEnd?: string;
}
interface JQuery {
    external(): any;
    watermark(watermark?: string, options?: WatermarkOptions): any;
}
