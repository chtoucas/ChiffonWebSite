// yepnope (incomplet)

interface YepNopeOptions {
  test?: boolean;
  yep?: string[];
  nope?: string[];
  load?: string[];
  callback?: (url: string, result: boolean, key: string) => void;
  complete: () => void;
}

declare function yepnope(options: YepNopeOptions): void;
