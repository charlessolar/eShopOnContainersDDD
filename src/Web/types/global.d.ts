/** Global definitions for developement **/

// for style loader
declare module '*.css' {
  const styles: any;
  export = styles;
}

declare const __DEV__: boolean;
declare const VERSION: string;
declare const COMMITHASH: string;
declare const TIMESTAMP: string;
declare const BRANCH: string;
declare const DATE: string;
declare const TIME: string;
declare const API_SERVER: string;
